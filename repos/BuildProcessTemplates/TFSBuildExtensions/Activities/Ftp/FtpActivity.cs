using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Build.Client;
using System.Activities;
using System.Diagnostics;
using System.Threading;
using System.Globalization;
using TFSBuildExtensions.Library.SynchronizeContent;
using Vueling.Activities.Impl.ServiceLibrary.Synchronization;
using WinSCP;

namespace TFSBuildExtensions.Ftp
{
    [BuildActivity(HostEnvironmentOption.All)]
    public class FtpActivity : BaseCodeActivity
    {
        [RequiredArgument]
        public InArgument<string> SourcesDirectory { get; set; }

        [RequiredArgument]
        public InArgument<string> FileMask { get; set; }

        [RequiredArgument]
        public InArgument<string> FtpSite { get; set; }

        [RequiredArgument]
        public InArgument<string> FtpUserName { get; set; }

        [RequiredArgument]
        public InArgument<string> FtpPassword { get; set; }

        public InArgument<string> FtpFolderPath { get; set; }

        public InArgument<bool> IgnoreErrors { get; set; }

        public OutArgument<bool> Result { get; set; }

        private SessionOptions sessionOptions;
        private TransferOptions transferOptions;
        private Session session;

        protected override void InternalExecute()
        {
            string ftpSite = this.FtpSite.Get(this.ActivityContext);
            string ftpUserName = this.FtpUserName.Get(this.ActivityContext);
            string ftpPassword = this.FtpPassword.Get(this.ActivityContext);
            string ftpFolderPath = this.FtpFolderPath.Get(this.ActivityContext);

            publishWebsite(ftpSite, ftpUserName, ftpPassword, ftpFolderPath);
        }

        private void publishWebsite(string ftpSite, string ftpUserName,
            string ftpPassword, string ftpFolderPath)
        {
            try
            {
                sessionOptions = new SessionOptions();
                sessionOptions.Protocol = Protocol.Ftp;
                sessionOptions.HostName = ftpSite;
                sessionOptions.PortNumber = 21;
                sessionOptions.Password = ftpPassword;
                sessionOptions.UserName = ftpUserName;
                sessionOptions.Timeout = new TimeSpan(0, 3, 0);

                transferOptions = new TransferOptions();
                transferOptions.TransferMode = TransferMode.Binary;
                transferOptions.FileMask = this.FileMask.Get(this.ActivityContext);

                try
                {
                    session = new Session();
                    session.Open(sessionOptions);
                }
                catch (SessionLocalException)
                {
                    throw;
                }

                if ((ftpFolderPath != null) && (!ftpFolderPath.Equals(String.Empty)))
                {
                    if (!session.FileExists(ftpFolderPath))
                    {
                        session.CreateDirectory(ftpFolderPath);
                    }
                }

                SynchronizationResult synchronizationResult;

                synchronizationResult = session.SynchronizeDirectories(SynchronizationMode.Remote,
                                                                        this.SourcesDirectory.Get(this.ActivityContext),
                                                                        ftpFolderPath, false, false, SynchronizationCriteria.Time,
                                                                        transferOptions);
            }
            catch (Exception ex)
            {
                LogBuildError("Error while syncronizing ftp content: " + ex.Message);
                throw ex;
            }
        }
    }
}