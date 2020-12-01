using System.Collections.Generic;
using Vueling.Activities.Sync.Impl.ServiceLibrary.FtpService.Models;

namespace Vueling.Activities.Sync.Impl.ServiceLibrary.ToolsServices.Compare
{
    public interface ICompareDirectoryService
    {
        SyncActions GetActions(List<DirectoryContent> source, List<DirectoryContent> target);
    }
}