using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using Vueling.Configuration.Library;
using Vueling.Messaging.RabbitMqEndpoint.Contracts.ServiceLibrary.Policies;
using Vueling.Messaging.RabbitMqEndpoint.Impl.ServiceLibrary.Consumers;
using Vueling.Messaging.RabbitMqEndpoint.Impl.ServiceLibrary.Consumers.Dispatching;
using Vueling.Messaging.RabbitMqEndpoint.Impl.ServiceLibrary.Endpoints;
using Vueling.Messaging.RabbitMqEndpoint.Impl.ServiceLibrary.Policies;
using Vueling.Messaging.RabbitMqEndpoint.Impl.ServiceLibrary.Policies.Detectors.BuiltIn;
using Vueling.XXX.Message.Commands;
using Vueling.XXX.Message.Events.Flights;

namespace Vueling.XXX.MessageConsumer.WindowsService
{
    public partial class ConsumerWindowsService : ServiceBase
    {
        private readonly IEndpointManager _endpointManager;

        public ConsumerWindowsService(IEndpointManager endpointManager)
        {
            _endpointManager = endpointManager;

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                FailPolicy.CreateFor(typeof(FlightRescheduled), typeof(FlightCancelled), typeof(RescheduleFlight))
                    .ClassifyAsTransient(new SqlTransientErrorDetector())
                    .OnTransientError(retryLimit: 3,
                        retryPeriod: TimeSpan.FromMinutes(1),
                        periodType: PeriodType.ExponentialBackoff,
                        exponent: 2,
                        retryLimitAction: FailAction.SendToFailExchange)
                    .OnPersistentError(FailAction.SendToFailExchange);

                Endpoint.InitializeAsConsumer(_endpointManager)
                    .HandleEvent<FlightRescheduled>()
                    .AndHandleEvent<FlightCancelled>()
                    .AndHandleCommand<RescheduleFlight>()
                    .WithSingleConsumer(perConsumerConcurrency: 1)
                    .Start();
            }
            catch (Exception ex)
            {
                Trace.TraceError("Could not start windows service: " + ex);
            }
        }

        protected override void OnStop()
        {
            try
            {
                Endpoint.Stop(TimeSpan.FromSeconds(60));
            }
            catch (Exception ex)
            {
                Trace.TraceError("Could not stop windows service: " + ex);
            }
        }


        //protected override void OnStart(string[] args)
        //{
        //    // Fail Policies determine what happens when you call Failed on the IEventProxy/ICommandProxy
        //    // If the error is transient, the below policy will send the message to be retried up to 3 times (4 attempts in total)
        //    // If the error is persistent, the message will be sent to the Failed Exchange where AM will identify how to respond

        //    FailPolicy.CreateFor(typeof(FlightRescheduled), typeof(FlightCancelled), typeof(RescheduleFlight))
        //        .ClassifyAsTransient(new SqlTransientErrorDetector())
        //        .OnTransientError(retryLimit: 3,
        //            retryPeriod: TimeSpan.FromMinutes(1),
        //            periodType: PeriodType.ExponentialBackoff,
        //            exponent: 2,
        //            retryLimitAction: FailAction.SendToFailExchange)
        //        .OnPersistentError(FailAction.SendToFailExchange);

        //    // The endpoint name should represent what the endpoint's responsibility is
        //    // The code below will create subscription to these events and ensure that
        //    // the event handlers (see the EventHandlers folder in this project) are invoked
        //    // Concurrency: The number of messages that will be processed in parallel. In this case we processed messages sequentially
        //    var endpointProperties = new EndpointProperties("FlightOperations.FlightManagementConsumer", VuelingEnvironment.Current.ApplicationId);
        //    Endpoint.InitializeAsConsumer(_consumerManager, endpointProperties)
        //        .HandleEvent<FlightRescheduled>()
        //        .AndHandleEvent<FlightCancelled>()
        //        .AndHandleCommand<RescheduleFlight>()
        //        .WithSingleConsumer(perConsumerConcurrency: 1)
        //        .Start();

        //    // CONCURRENCY GUIDANCE
        //    // The version below is able to process 10 messages concurrently and new dependencies are instantiated for each message
        //    // so code will be thread-safe unless you use a singleton somewhere. Ensure EntityFramework is not SingleInstance!

        //    //var endpointProperties = new EndpointProperties("FlightOperations.FlightManagementConsumer", VuelingEnvironment.Current.ApplicationId);
        //    //Endpoint.InitializeAsConsumer(_consumerManager, endpointProperties)
        //    //    .HandleEvent<FlightRescheduled>(LifetimeScope.PerMessage)
        //    //    .AndHandleEvent<FlightCancelled>(LifetimeScope.PerMessage)
        //    //    .WithSingleConsumer(perConsumerConcurrency: 10)
        //    //    .Start();
        //}

        //protected override void OnStop()
        //{
        //    // Gives consumers 60 seconds to complete the processing of existing messages
        //    // After 60 seconds the consumers are forcibly shutdown if they haven't completed yet. Messages will be requeued.
        //    Endpoint.Stop(TimeSpan.FromSeconds(60));
        //}

        public void Start()
        {
            OnStart(null);

            Console.WriteLine("Started. Press any key to shutdown the consumer(s)");
            Console.Read();
            Console.WriteLine("Shutting down, this may take a few seconds...");
            Endpoint.Stop(TimeSpan.FromSeconds(60));
        }
    }
}
