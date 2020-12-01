﻿using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Activities;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TFSBuildExtensions.PowerShell
{
    [BuildActivity(HostEnvironmentOption.Agent)]
    public sealed class InvokePowershellCommandAsync : AsyncCodeActivity<PSObject[]>
    {
        /// <summary>
        /// Interface is used to allow use to mock out calls to the TFS server for testing
        /// </summary>
        private IUtilitiesForPowerShellActivity powershellUtilities;

        /// <summary>
        /// Initializes a new instance of the InvokePowerShellCommand class
        /// </summary>
        public InvokePowershellCommandAsync()
            : this(new UtilitiesForPowerShellActivity())
        {
        }

        /// <summary>
        /// Initializes a new instance of the InvokePowerShellCommand class
        /// </summary>
        /// <param name="powershellUtilities">Allows a mock implementation of utilities to be passed in for testing</param>
        internal InvokePowershellCommandAsync(IUtilitiesForPowerShellActivity powershellUtilities)
        {
            this.powershellUtilities = powershellUtilities;
        }

        /// <summary>
        /// Gets or sets the powershell command script to execute.
        /// </summary>
        /// <value>The command script in string form</value>
        [RequiredArgument]
        [Browsable(true)]
        public InArgument<string> Script { get; set; }

        /// <summary>
        /// Gets or sets any arguments to be provided to the script
        /// <value>An arguments list for the command as a string</value>
        /// </summary>
        [Browsable(true)]
        public InArgument<string> Arguments { get; set; }

        /// <summary>
        /// Gets or sets the build workspace. This is used to obtain
        /// a powershell script from a source control path
        /// </summary>
        /// <value>The workspace used by the current build</value>
        [Browsable(true)]
        [DefaultValue(null)]
        public InArgument<Workspace> BuildWorkspace { get; set; }

        /// <summary>
        /// Resolves the provided script parameter to either a server stored 
        /// PS file or an inline script for direct execution.
        /// </summary>
        /// <param name="workspace">The TFS workspace</param>
        /// <param name="script">The powershell script or path</param>
        /// <param name="arguments">The powershell script arguments</param>
        /// <returns>An executable powershell command</returns>
        internal string ResolveScript(Workspace workspace, string script, string arguments)
        {
            if (string.IsNullOrWhiteSpace(script))
            {
                throw new ArgumentNullException("script");
            }

            if (this.powershellUtilities.IsServerItem(script))
            {
                Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Script being read from TFS source control path [{0}] and attributes [{1}] being used", script, arguments));

                var workspaceFilePath = this.powershellUtilities.GetLocalFilePathFromWorkspace(workspace, script);

                if (!this.powershellUtilities.FileExists(workspaceFilePath))
                {
                    throw new FileNotFoundException("Script", string.Format(CultureInfo.CurrentCulture, "Workspace local path [{0}] for source path [{1}] was not found", script, workspaceFilePath));
                }

                script = string.Format("& '{0}' {1}", workspaceFilePath, arguments);
            }
            else if (this.powershellUtilities.FileExists(script))
            {
                Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Script being read from local file path [{0}] and attributes [{1}] being used", script, arguments));
                script = string.Format("& '{0}' {1}", script, arguments);
            }
            else
            {
                Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "In-line script, attributes being ignored [{0}]", script));
            }

            return script;
        }

        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            Runspace runspace = null;
            Pipeline pipeline = null;

            var script = this.ResolveScript(
              this.BuildWorkspace.Get(context),
              this.Script.Get(context),
              this.Arguments.Get(context));

            context.Track(new System.Activities.Tracking.CustomTrackingRecord(string.Format(CultureInfo.CurrentCulture, "Script resolved to {0}", script)));
            //context.TrackBuildMessage(string.Format(CultureInfo.CurrentCulture, "Script resolved to {0}", script), BuildMessageImportance.Low);

            try
            {
                runspace = RunspaceFactory.CreateRunspace();
                runspace.Open();
                pipeline = runspace.CreatePipeline(script);

                context.UserState = pipeline;
                return new PipelineInvokerAsyncResult(pipeline, callback, state);
            }
            catch
            {
                if (runspace != null)
                {
                    runspace.Dispose();
                }

                if (pipeline != null)
                {
                    pipeline.Dispose();
                }

                throw;
            }
        }

        protected override PSObject[] EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            PipelineInvokerAsyncResult asyncResult = result as PipelineInvokerAsyncResult;
            Pipeline pipeline = context.UserState as Pipeline;

            try
            {
                if (asyncResult.Exception != null)
                {
                    throw new PowerShellExecutionException(asyncResult.Exception, asyncResult.ErrorRecords);
                }

                return asyncResult.PipelineOutput.ToArray();
            }
            finally
            {
                DisposePipeline(pipeline);
            }
        }

        void DisposePipeline(Pipeline pipelineInstance)
        {
            if (pipelineInstance != null)
            {
                pipelineInstance.Runspace.Close();
                pipelineInstance.Dispose();
            }
            pipelineInstance = null;
        }
    }

    // An instance of this object is returned to the runtime upon beginning
    // execution.  This also keeps a pointer to a callback function, which
    // it calls when it completes.
    class PipelineInvokerAsyncResult : IAsyncResult
    {
        AsyncCallback callback;
        object asyncState;
        EventWaitHandle asyncWaitHandle;

        Collection<ErrorRecord> errorRecords;
        public Collection<ErrorRecord> ErrorRecords
        {
            get
            {
                if (this.errorRecords == null)
                {
                    this.errorRecords = new Collection<ErrorRecord>();
                }

                return this.errorRecords;
            }
        }

        public Exception Exception
        {
            get;
            set;
        }

        public Collection<PSObject> PipelineOutput
        {
            get;
            set;
        }

        public object AsyncState
        {
            get { return this.asyncState; }
        }

        public WaitHandle AsyncWaitHandle
        {
            get { return this.asyncWaitHandle; }
        }

        public bool CompletedSynchronously
        {
            get { return false; }
        }

        public bool IsCompleted
        {
            get { return true; }
        }

        public PipelineInvokerAsyncResult(Pipeline pipeline, AsyncCallback callback, object state)
        {
            this.asyncState = state;
            this.callback = callback;
            this.asyncWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
            pipeline.StateChanged += new EventHandler<PipelineStateEventArgs>(OnStateChanged);
            pipeline.InvokeAsync();
        }

        void Complete()
        {
            this.asyncWaitHandle.Set();
            if (this.callback != null)
            {
                this.callback(this);
            }
        }

        // Called by the underlying PowerShell pipeline object on state changes.
        void OnStateChanged(object sender, PipelineStateEventArgs args)
        {
            try
            {
                PipelineState state = args.PipelineStateInfo.State;
                Pipeline pipeline = sender as Pipeline;

                if (state == PipelineState.Completed)
                {
                    this.PipelineOutput = pipeline.Output.ReadToEnd();
                    ReadErrorRecords(pipeline);
                    Complete();
                }
                else if (state == PipelineState.Failed)
                {
                    this.Exception = args.PipelineStateInfo.Reason;
                    ReadErrorRecords(pipeline);
                    Complete();
                }
                else if (state == PipelineState.Stopped)
                {
                    Complete(); ;
                }
                else
                {
                    return; // nothing to do
                }
            }
            catch (Exception e)
            {
                this.Exception = e;
                Complete();
            }
        }

        void ReadErrorRecords(Pipeline pipeline)
        {
            Collection<object> errorsRecords = pipeline.Error.ReadToEnd();
            if (errorsRecords.Count != 0)
            {
                foreach (PSObject item in errorsRecords)
                {
                    ErrorRecord errorRecord = item.BaseObject as ErrorRecord;
                    this.ErrorRecords.Add(errorRecord);
                }
            }
        }
    }

}
