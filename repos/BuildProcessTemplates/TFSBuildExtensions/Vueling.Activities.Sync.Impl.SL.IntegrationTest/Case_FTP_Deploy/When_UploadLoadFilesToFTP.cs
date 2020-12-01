using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vueling.Activities.Sync.Contracts.ServiceLibrary.Synchronization;
using Vueling.Activities.Sync.Impl.ServiceLibrary.FtpService;
using TFSBuildExtensions.Library.SynchronizeContent;
using Vueling.Activities.Sync.Impl.ServiceLibrary.ToolsServices.FtpDiscovery;
using Vueling.Activities.Sync.Impl.ServiceLibrary.ToolsServices.LocalDiscovery;
using Vueling.Activities.Sync.Impl.ServiceLibrary.ToolsServices.Compare;
using System.Collections.Generic;
using System.IO;

namespace Vueling.Activities.Sync.Impl.SL.IntegrationTest.Case_FTP_Deploy
{
    [TestClass]
    public class When_UploadLoadFilesToFTP
    {
        [TestMethod]
        public void Then_Files_Uploaded_Ok()
        {
            SynchronizeContentConfiguration _synchronizeContentConfiguration = new SynchronizeContentConfiguration();

            _synchronizeContentConfiguration.SourceSettings = new SourceSettings();
            
            _synchronizeContentConfiguration.SourceSettings.RootPath = @"C:\Temp\Vueling.MobileWeb.WebUI";
            
            _synchronizeContentConfiguration.SourceSettings.SourceType = SourceTypeEnum.LOCAL;

            _synchronizeContentConfiguration.TargetSettings = new TargetSettings();
            _synchronizeContentConfiguration.TargetSettings.RootPath = "";
            _synchronizeContentConfiguration.TargetSettings.SourceType = SourceTypeEnum.FTP;
            _synchronizeContentConfiguration.TargetSettings.FtpSettings = new FtpSettings();
            _synchronizeContentConfiguration.TargetSettings.FtpSettings.ServerAddress = "ftp://nsvueling.upload.akamai.com";
            _synchronizeContentConfiguration.TargetSettings.FtpSettings.User = "vlgmobile_tfs_int";
            _synchronizeContentConfiguration.TargetSettings.FtpSettings.Password = "M0b1l3_TF5_int_H5Wn";
            _synchronizeContentConfiguration.TargetSettings.FtpSettings.StartingPath = "";
            _synchronizeContentConfiguration.TargetSettings.FtpSettings.FtpSyncType = FtpSyncTypeEnum.LOCAL_WITH_DELETE;

            _synchronizeContentConfiguration.ExcludeContent = new List<string> {".*", "*.csproj","*.csproj.user","*.csproj.vspscc","Web.Release.config","Web.Debug.config","Web.config","EnsureNugetPackages.*","ExecutePrebuildTasks.*"};

            IFtpDirectoryService _ftpDirectoryService = new FtpDirectoryService();
            IFileDirectoryService _fileDirectoryService = new FileDirectoryService();
            ICompareDirectoryService _compareDirectoryService = new CompareDirectoryService();

            ISynchronizationService synchronizationService = new FtpSynchronizationService(_synchronizeContentConfiguration, _ftpDirectoryService, _fileDirectoryService, _compareDirectoryService);

            synchronizationService.RunSynchronization();
        }
    }
}
