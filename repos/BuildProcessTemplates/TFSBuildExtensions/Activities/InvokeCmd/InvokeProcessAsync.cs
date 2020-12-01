using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Build.Workflow.Activities;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TFSBuildExtensions.InvokeCmd
{
    [BuildActivity(HostEnvironmentOption.All)]
    public class InvokeProcessAsync : AsyncCodeActivity<string>
    {
        ProcessStartInfo psi;
        Process process;

        [Browsable(true)]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public InArgument<string> Arguments { get; set; }

        [Browsable(true)]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public InArgument<System.Collections.Generic.IDictionary<string, string>> EnvironmentVariables { get; set; }

        [Browsable(false)]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ActivityAction<string> ErrorDataReceived { get; set; }

        [Browsable(true)]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]

        [RequiredArgument]
        public InArgument<string> FileName { get; set; }
        [Browsable(false)]

        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public ActivityAction<string> OutputDataReceived { get; set; }

        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public InArgument<System.Text.Encoding> OutputEncoding { get; set; }

        [Browsable(true)]
        [DefaultValue("")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public InArgument<string> WorkingDirectory { get; set; }

        protected override IAsyncResult BeginExecute(AsyncCodeActivityContext context, AsyncCallback callback, object state)
        {
            psi = new ProcessStartInfo {
                FileName = this.FileName.Get(context),
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = this.Arguments.Get(context),
                WorkingDirectory = this.WorkingDirectory.Get(context)
            };

            process = new Process {
                StartInfo = psi
            };

            return new InvokeProcessAsyncResult(process, callback, state);
        }

        protected override string EndExecute(AsyncCodeActivityContext context, IAsyncResult result)
        {
            InvokeProcessAsyncResult asyncResult = result as InvokeProcessAsyncResult;
            Process process = context.UserState as Process;

            try
            {
                if (!String.IsNullOrEmpty(asyncResult.DataError.ToString()))
                {
                    throw new Exception(asyncResult.DataError.ToString());
                }

                return asyncResult.DataOutput.ToString();
            }
            finally
            {
                process.Close();
            }
        }

        class InvokeProcessAsyncResult : IAsyncResult
        {
            AsyncCallback callback;
            object asyncState;
            EventWaitHandle asyncWaitHandle;
            Process process;

            private StringBuilder dataOutput = new StringBuilder();

            public StringBuilder DataOutput
            {
                get { return dataOutput; }
                set { dataOutput = value; }
            }
            private StringBuilder dataError = new StringBuilder();

            public StringBuilder DataError
            {
                get { return dataError; }
                set { dataError = value; }
            }

            public InvokeProcessAsyncResult(Process _process, AsyncCallback callback, object state)
            {
                this.asyncState = state;
                this.callback = callback;
                this.asyncWaitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);

                process = _process;

                process.OutputDataReceived += new DataReceivedEventHandler(OutputDataHandler);
                process.ErrorDataReceived += new DataReceivedEventHandler(ErrorDataHandler);
            }

            internal virtual void Process()
            {
                process.Start();
                process.WaitForExit();
            }

            private void OutputDataHandler(object sendingProcess,
            DataReceivedEventArgs outLine)
            {
                if (!String.IsNullOrEmpty(outLine.Data))
                {
                    dataOutput.Append(Environment.NewLine + outLine.Data);
                }
            }

            private void ErrorDataHandler(object sendingProcess,
            DataReceivedEventArgs outLine)
            {
                if (!String.IsNullOrEmpty(outLine.Data))
                {
                    dataError.Append(Environment.NewLine + outLine.Data);
                }
            }

            public object AsyncState
            {
                get { return this.asyncState; }
            }

            public System.Threading.WaitHandle AsyncWaitHandle
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
        }
    }
}