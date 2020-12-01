﻿using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
#if CORECLR
using System.Runtime.Loader;
#endif
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Amazon.CloudWatchLogs;
using Amazon.CloudWatchLogs.Model;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Newtonsoft.Json;
using System.Net;
namespace AWS.Logger.Core
{
    public class AWSLoggerCore : IAWSLoggerCore
    {
        const int MAX_MESSAGE_SIZE_IN_BYTES = 256000;

        #region Private Members
        const string EMPTY_MESSAGE = "\t";
        private ConcurrentQueue<InputLogEvent> _pendingMessageQueue = new ConcurrentQueue<InputLogEvent>();
        private string _currentStreamName = null;
        private LogEventBatch _repo = new LogEventBatch();
        private CancellationTokenSource _cancelStartSource;
        private SemaphoreSlim _flushTriggerEvent;
        private ManualResetEventSlim _flushCompletedEvent;
        private AWSLoggerConfig _config;
        private IAmazonCloudWatchLogs _client;
        private DateTime _maxBufferTimeStamp = new DateTime();
        private string _logType;
        private int _requestCount = 5;
        const double MAX_BUFFER_TIMEDIFF = 5;
        private readonly static Regex invalid_sequence_token_regex = new
            Regex(@"The given sequenceToken is invalid. The next expected sequenceToken is: (\d+)");

        public sealed class LogLibraryEventArgs : EventArgs
        {
            internal LogLibraryEventArgs(Exception ex)
            {
                Exception = ex;
            }

            public Exception Exception { get; }
            public string ServiceUrl { get; internal set; }
        }

        public event EventHandler<LogLibraryEventArgs> LogLibraryAlert;

        #endregion
        public AWSLoggerCore(AWSLoggerConfig config, string logType)
        {
            _config = config;
            _logType = logType;

            var credentials = DetermineCredentials(config);

            if (_config.Region != null)
            {
                _client = new AmazonCloudWatchLogsClient(credentials, Amazon.RegionEndpoint.GetBySystemName(_config.Region));
            }
            else
            {
                _client = new AmazonCloudWatchLogsClient(credentials);
            }

            ((AmazonCloudWatchLogsClient)this._client).BeforeRequestEvent += ServiceClientBeforeRequestEvent;
            ((AmazonCloudWatchLogsClient)this._client).ExceptionEvent += ServiceClienExceptionEvent;

            StartMonitor();
            RegisterShutdownHook();
        }


#if CORECLR
        private void RegisterShutdownHook()
        {
            var currentAssembly = typeof(AWSLoggerCore).GetTypeInfo().Assembly;
            AssemblyLoadContext.GetLoadContext(currentAssembly).Unloading += this.OnAssemblyLoadContextUnloading;
        }

        internal void OnAssemblyLoadContextUnloading(AssemblyLoadContext obj)
        {
            this.Close();
        }
#else
        private void RegisterShutdownHook()
        {
            AppDomain.CurrentDomain.DomainUnload += ProcessExit;
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;
        }

        private void ProcessExit(object sender, EventArgs e)
        {
            Close();
        }
#endif

        private static AWSCredentials DetermineCredentials(AWSLoggerConfig config)
        {
            if (config.Credentials != null)
            {
                return config.Credentials;
            }
            if (!string.IsNullOrEmpty(config.Profile) && new CredentialProfileStoreChain(config.ProfilesLocation)
                .TryGetAWSCredentials(config.Profile, out var credentials))
            {
                return credentials;
            }
            return FallbackCredentialsFactory.GetCredentials();
        }

        public void Close()
        {
            try
            {
                Flush();
                _cancelStartSource.Cancel();
            }
            catch (Exception ex)
            {
                LogLibraryServiceError(ex);
            }
            finally
            {
                LogLibraryAlert = null;
            }
        }

        public void Flush()
        {
            if (_cancelStartSource.IsCancellationRequested)
                return;

            if (!_pendingMessageQueue.IsEmpty || !_repo.IsEmpty)
            {
                bool lockTaken = false;
                try
                {
                    // Ensure only one thread executes the flush operation
                    System.Threading.Monitor.TryEnter(_flushTriggerEvent, ref lockTaken);
                    if (lockTaken)
                    {
                        _flushCompletedEvent.Reset();
                        if (_flushTriggerEvent.CurrentCount == 0)
                        {
                            _flushTriggerEvent.Release();   // Signal Monitor-Task to start premature flush
                        }
                        else
                        {
                            // Means that the Background Task is busy, and not yet claimed the previous release (Maybe busy with credentials)
                            var serviceUrl = GetServiceUrl();
                            LogLibraryServiceError(new TimeoutException($"Flush Pending - ServiceURL={serviceUrl}, StreamName={_currentStreamName}, PendingMessages={_pendingMessageQueue.Count}, CurrentBatch={_repo.CurrentBatchMessageCount}"), serviceUrl);
                        }
                    }

                    // Waiting for Monitor-Task to complete flush
                    if (!_flushCompletedEvent.Wait(TimeSpan.FromSeconds(30), _cancelStartSource.Token))
                    {
                        var serviceUrl = GetServiceUrl();
                        LogLibraryServiceError(new TimeoutException($"Flush Timeout - ServiceURL={serviceUrl}, StreamName={_currentStreamName}, PendingMessages={_pendingMessageQueue.Count}, CurrentBatch={_repo.CurrentBatchMessageCount}"), serviceUrl);
                    }
                }
                finally
                {
                    if (lockTaken)
                        System.Threading.Monitor.Exit(_flushTriggerEvent);
                }
            }
        }

        private string GetServiceUrl()
        {
            try
            {
                _client.Config.Validate();
                return _client.Config.DetermineServiceURL() ?? "Undetermined ServiceURL";
            }
            catch (Exception ex)
            {
                LogLibraryServiceError(ex, string.Empty);
                return "Unknown ServiceURL";
            }
        }

        private void AddSingleMessage(string message)
        {
            
            if (_pendingMessageQueue.Count > _config.MaxQueuedMessages)
            {
                if (_maxBufferTimeStamp.AddMinutes(MAX_BUFFER_TIMEDIFF) < DateTime.UtcNow)
                {
                    message = "The AWS Logger in-memory buffer has reached maximum capacity";
                    if (_maxBufferTimeStamp == DateTime.MinValue)
                    {
                        LogLibraryServiceError(new System.InvalidOperationException(message));
                    }
                    _maxBufferTimeStamp = DateTime.UtcNow;
                    _pendingMessageQueue.Enqueue(new InputLogEvent
                    {
                        Timestamp = DateTime.Now,
                        Message = message,
                    });
                }
            }
            else
            {
                var currentAssembly = typeof(AWSLoggerCore).GetTypeInfo().Assembly;
                LogMessageMetadata meta = new LogMessageMetadata();
                meta.Ami = Environment.GetEnvironmentVariable("EC2_AMI_ID")!=null ? Environment.GetEnvironmentVariable("EC2_AMI_ID"): "NO_AMI_ID";
                meta.AtcLog = "2.0.0.0 ### LOG4NET" + currentAssembly.GetName().Version.ToString();
                meta.Environment = System.Environment.GetEnvironmentVariable("APP_ENVIRONMENT")!=null ? System.Environment.GetEnvironmentVariable("APP_ENVIRONMENT") : System.Environment.MachineName;
                meta.MachineName = System.Environment.MachineName;
              


                LogMessage log= new LogMessage();
                string date;
                //log.Metadata = new List<LogMessageMetadata>();
                log.Metadata = new LogMessageMetadata();
                string[] messageSplit = message.Split('*');

                date=DateTime.UtcNow.ToString("yyyy - MM - ddTHH\\:mm\\:ss.ffffff") + "Z";
                log.timestamp = date;
                log.Level = messageSplit[1];
                log.Application= messageSplit[2];
                log.LogEventType = "General";
                //log.Metadata.Add(meta); 
                log.Metadata = (LogMessageMetadata)meta;
                log.Message= messageSplit[3];
                message= JsonConvert.SerializeObject(log);
               
                _pendingMessageQueue.Enqueue(new InputLogEvent
                {
                    Timestamp = DateTime.Now,
                    Message = message,
                });
            }
        }

        /// <summary>
        /// A Concurrent Queue is used to store the messages from 
        /// the logger
        /// </summary>
        /// <param name="rawMessage"></param>
        public void AddMessage(string rawMessage)
        {
           
            if (string.IsNullOrEmpty(rawMessage))
            {
                rawMessage = EMPTY_MESSAGE;
            }

            // Only do the extra work of breaking up the message if the max unicode bytes exceeds the possible size. This is not
            // an exact measurement since the string is UTF8 but it gives us a chance to skip the extra computation for 
            // typically small messages.
            if (Encoding.Unicode.GetMaxByteCount(rawMessage.Length) < MAX_MESSAGE_SIZE_IN_BYTES)
            {
                AddSingleMessage(rawMessage);
            }
            else
            {
                var messageParts = BreakupMessage(rawMessage);
               
                foreach (var message in messageParts)
                {
                    AddSingleMessage(message);
                }
            }
        }

        ~AWSLoggerCore()
        {
            if (_cancelStartSource != null)
            {
                _cancelStartSource.Dispose();
            }
        }

        /// <summary>
        /// Kicks off the Poller Thread to keep tabs on the PutLogEvent request and the
        /// Concurrent Queue
        /// </summary>
        /// <param name="PatrolSleepTime"></param>
        public void StartMonitor()
        {
            _flushTriggerEvent = new SemaphoreSlim(0, 1);
            _flushCompletedEvent = new ManualResetEventSlim(false);
            _cancelStartSource = new CancellationTokenSource();
            Task.Run(async () =>
            {
                await Monitor(_cancelStartSource.Token);
            });
        }

        /// <summary>
        /// Patrolling thread. keeps tab on the PutLogEvent request and the
        /// Concurrent Queue
        /// </summary>
        private async Task Monitor(CancellationToken token)
        {
            bool executeFlush = false;

            while (_currentStreamName == null && !token.IsCancellationRequested)
            {
                try
                {
                    _currentStreamName = await LogEventTransmissionSetup(token).ConfigureAwait(false);
                }
                catch (OperationCanceledException ex)
                {
                    if (!_pendingMessageQueue.IsEmpty)
                        LogLibraryServiceError(ex);
                    if (token.IsCancellationRequested)
                    {
                        _client.Dispose();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    // We don't want to kill the main monitor loop. We will simply log the error, then continue.
                    // If it is an OperationCancelledException, die
                    LogLibraryServiceError(ex);
                    await Task.Delay(Math.Max(100, DateTime.UtcNow.Second * 10), token);
                }
            }

            while (!token.IsCancellationRequested)
            {
                try
                {
                    while (_pendingMessageQueue.TryDequeue(out var inputLogEvent))
                    {
                        // See if new message will cause the current batch to violote the size constraint.
                        // If so send the current batch now before adding more to the batch of messages to send.
                        if (_repo.CurrentBatchMessageCount > 0 && _repo.IsSizeConstraintViolated(inputLogEvent.Message))
                        {
                            await SendMessages(token).ConfigureAwait(false);
                        }

                        _repo.AddMessage(inputLogEvent);
                    }

                    if (_repo.ShouldSendRequest(_config.MaxQueuedMessages) || (executeFlush && !_repo.IsEmpty))
                    {
                        await SendMessages(token).ConfigureAwait(false);
                    }

                    if (executeFlush)
                        _flushCompletedEvent.Set();

                    executeFlush = await _flushTriggerEvent.WaitAsync(TimeSpan.FromMilliseconds(_config.MonitorSleepTime.TotalMilliseconds), token);
                }
                catch (OperationCanceledException ex)
                {
                    if (!token.IsCancellationRequested || !_repo.IsEmpty || !_pendingMessageQueue.IsEmpty)
                        LogLibraryServiceError(ex);
                    _client.Dispose();
                    return;
                }
                catch (Exception ex)
                {
                    // We don't want to kill the main monitor loop. We will simply log the error, then continue.
                    // If it is an OperationCancelledException, die
                    LogLibraryServiceError(ex);
                }
            }
        }

        /// <summary>
        /// Method to transmit the PutLogEvent Request
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task SendMessages(CancellationToken token)
        {
            try
            {
                //Make sure the log events are in the right order.
                _repo._request.LogEvents.Sort((ev1, ev2) => ev1.Timestamp.CompareTo(ev2.Timestamp));
                var response = await _client.PutLogEventsAsync(_repo._request, token).ConfigureAwait(false);
                _repo.Reset(response.NextSequenceToken);
                _requestCount = 5;
            }
            catch (InvalidSequenceTokenException ex)
            {
                //In case the NextSequenceToken is invalid for the last sent message, a new stream would be 
                //created for the said application.
                LogLibraryServiceError(ex);
                if (_requestCount > 0)
                {
                    _requestCount--;
                    var regexResult = invalid_sequence_token_regex.Match(ex.Message);
                    if (regexResult.Success)
                    {
                        _repo._request.SequenceToken = regexResult.Groups[1].Value;
                        await SendMessages(token).ConfigureAwait(false);
                    }
                }
                else
                {
                    _currentStreamName = await LogEventTransmissionSetup(token).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        /// Creates and Allocates resources for message trasnmission
        /// </summary>
        /// <returns></returns>
        private async Task<string> LogEventTransmissionSetup(CancellationToken token)
        {
            string serviceURL = GetServiceUrl();
            var logGroupResponse = await _client.DescribeLogGroupsAsync(new DescribeLogGroupsRequest
            {
                LogGroupNamePrefix = _config.LogGroup
            }, token).ConfigureAwait(false);
            if (!IsSuccessStatusCode(logGroupResponse))
            {
                LogLibraryServiceError(new System.Net.WebException($"Lookup LogGroup {_config.LogGroup} returned status: {logGroupResponse.HttpStatusCode}"), serviceURL);
            }

            if (logGroupResponse.LogGroups.FirstOrDefault(x => string.Equals(x.LogGroupName, _config.LogGroup, StringComparison.Ordinal)) == null)
            {
                var createGroupResponse = await _client.CreateLogGroupAsync(new CreateLogGroupRequest { LogGroupName = _config.LogGroup }, token).ConfigureAwait(false);
                if (!IsSuccessStatusCode(createGroupResponse))
                {
                    LogLibraryServiceError(new System.Net.WebException($"Create LogGroup {_config.LogGroup} returned status: {createGroupResponse.HttpStatusCode}"), serviceURL);
                }
            }

            var currentStreamName = DateTime.Now.ToString("yyyy/MM/ddTHH.mm.ss") + " - " + _config.LogStreamNameSuffix;

            var streamResponse = await _client.CreateLogStreamAsync(new CreateLogStreamRequest
            {
                LogGroupName = _config.LogGroup,
                LogStreamName = currentStreamName
            }, token).ConfigureAwait(false);
            if (!IsSuccessStatusCode(streamResponse))
            {
                LogLibraryServiceError(new System.Net.WebException($"Create LogStream {currentStreamName} for LogGroup {_config.LogGroup} returned status: {streamResponse.HttpStatusCode}"), serviceURL);
            }

            _repo = new LogEventBatch(_config.LogGroup, currentStreamName, Convert.ToInt32(_config.BatchPushInterval.TotalSeconds), _config.BatchSizeInBytes);

            return currentStreamName;
        }

        private static bool IsSuccessStatusCode(AmazonWebServiceResponse serviceResponse)
        {
            return (int)serviceResponse.HttpStatusCode >= 200 && (int)serviceResponse.HttpStatusCode <= 299;
        }

        /// <summary>
        /// Break up the message into max parts of 256K.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static IList<string> BreakupMessage(string message)
        {
            var parts = new List<string>();

            var singleCharArray = new char[1];
            var encoding = Encoding.UTF8;
            int byteCount = 0;
            var sb = new StringBuilder(MAX_MESSAGE_SIZE_IN_BYTES);
            foreach (var c in message)
            {
                singleCharArray[0] = c;
                byteCount += encoding.GetByteCount(singleCharArray);
                sb.Append(c);

                // This could go a couple bytes
                if (byteCount > MAX_MESSAGE_SIZE_IN_BYTES)
                {
                    parts.Add(sb.ToString());
                    sb.Clear();
                    byteCount = 0;
                }
            }

            if (sb.Length > 0)
            {
                parts.Add(sb.ToString());
            }

            return parts;
        }

        /// <summary>
        /// Class to handle PutLogEvent request and associated parameters. 
        /// Also has the requisite checks to determine when the object is ready for Transmission.
        /// </summary>
        private class LogEventBatch
        {
            public TimeSpan TimeIntervalBetweenPushes { get; private set; }
            public int MaxBatchSize { get; private set; }

            public bool ShouldSendRequest(int maxQueuedEvents)
            {
                if (_request.LogEvents.Count == 0)
                    return false;

                if (_nextPushTime < DateTime.UtcNow)
                    return true;

                if (maxQueuedEvents <= _request.LogEvents.Count)
                    return true;

                return false;
            }

            int _totalMessageSize { get; set; }
            DateTime _nextPushTime;
            public PutLogEventsRequest _request = new PutLogEventsRequest();
            public LogEventBatch(string logGroupName, string streamName, int timeIntervalBetweenPushes, int maxBatchSize)
            {
                _request.LogGroupName = logGroupName;
                _request.LogStreamName = streamName;
                TimeIntervalBetweenPushes = TimeSpan.FromSeconds(timeIntervalBetweenPushes);
                MaxBatchSize = maxBatchSize;
                Reset(null);
            }

            public LogEventBatch()
            {
            }

            public int CurrentBatchMessageCount
            {
                get { return this._request.LogEvents.Count; }
            }

            public bool IsEmpty => _request.LogEvents.Count == 0;

            public bool IsSizeConstraintViolated(string message)
            {
                Encoding unicode = Encoding.Unicode;
                int prospectiveLength = _totalMessageSize + unicode.GetMaxByteCount(message.Length);
                if (MaxBatchSize < prospectiveLength)
                    return true;

                return false;
            }

            public void AddMessage(InputLogEvent ev)
            {
                Encoding unicode = Encoding.Unicode;
                _totalMessageSize += unicode.GetMaxByteCount(ev.Message.Length);
                _request.LogEvents.Add(ev);
            }

            public void Reset(string SeqToken)
            {
                _request.LogEvents.Clear();
                _totalMessageSize = 0;
                _request.SequenceToken = SeqToken;
                _nextPushTime = DateTime.UtcNow.Add(TimeIntervalBetweenPushes);
            }
        }

        const string UserAgentHeader = "User-Agent";
        void ServiceClientBeforeRequestEvent(object sender, RequestEventArgs e)
        {
            var args = e as Amazon.Runtime.WebServiceRequestEventArgs;
            if (args == null || !args.Headers.ContainsKey(UserAgentHeader))
                return;

            args.Headers[UserAgentHeader] = args.Headers[UserAgentHeader] + " AWSLogger/" + _logType;
        }

        void ServiceClienExceptionEvent(object sender, ExceptionEventArgs e)
        {
            var eventArgs = e as WebServiceExceptionEventArgs;
            if (eventArgs?.Exception != null)
                LogLibraryServiceError(eventArgs?.Exception, eventArgs.Endpoint?.ToString());
            else
                LogLibraryServiceError(new System.Net.WebException(e.GetType().ToString()));
        }

        private void LogLibraryServiceError(Exception ex, string serviceUrl = null)
        {
            LogLibraryAlert?.Invoke(this, new LogLibraryEventArgs(ex) { ServiceUrl = serviceUrl ?? GetServiceUrl() } );
            if (!string.IsNullOrEmpty(_config.LibraryLogFileName))
            {
                LogLibraryError(ex, _config.LibraryLogFileName);
            }
        }

        public static void LogLibraryError(Exception ex, string LibraryLogFileName)
        {
            try
            {
                using (StreamWriter w = File.AppendText(LibraryLogFileName))
                {
                    w.WriteLine("Log Entry : ");
                    w.WriteLine("{0}", DateTime.Now.ToString());
                    w.WriteLine("  :");
                    w.WriteLine("  :{0}", ex.ToString());
                    w.WriteLine("-------------------------------");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught when writing error log to file" + e.ToString());
            }
        }
    }
}
