using System.Collections.Generic;
using Vueling.Activities.Sync.Impl.ServiceLibrary.FtpService.Models;

namespace Vueling.Activities.Sync.Impl.ServiceLibrary.ToolsServices.LocalDiscovery
{
    public interface IFileDirectoryService
    {
        List<DirectoryContent> GetDirectoryInfo(string path, List<string> excludeContent);
    }
}