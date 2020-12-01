using System.Collections.Generic;
using TFSBuildExtensions.Library.SynchronizeContent;
using Vueling.Activities.Sync.Impl.ServiceLibrary.FtpService.Models;

namespace Vueling.Activities.Sync.Impl.ServiceLibrary.ToolsServices.FtpDiscovery
{
    public interface IFtpDirectoryService
    {
        List<DirectoryContent> GetDirectoryInfo(FtpSettings ftpSettings, List<string> excludeContent);

        bool ExistsStartingFolder(FtpSettings ftpSettings);
    }
}