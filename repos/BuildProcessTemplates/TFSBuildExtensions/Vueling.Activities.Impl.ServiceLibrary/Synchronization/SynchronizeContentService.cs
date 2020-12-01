using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSBuildExtensions.Library.SynchronizeContent;
using Vueling.Activities.Contracts.ServiceLibrary.Synchronization;
using Vueling.Activities.Sync.Contracts.ServiceLibrary.Synchronization;
using Vueling.Activities.Sync.Impl.ServiceLibrary.FtpService;
using Vueling.Activities.Sync.Impl.ServiceLibrary.ToolsServices.Compare;
using Vueling.Activities.Sync.Impl.ServiceLibrary.ToolsServices.FtpDiscovery;
using Vueling.Activities.Sync.Impl.ServiceLibrary.ToolsServices.LocalDiscovery;
using Vueling.Extensions.Library.DI;

namespace Vueling.Activities.Impl.ServiceLibrary.Synchronization
{
    [RegisterServiceAttribute]
    public class SynchronizeContentService : BaseActivityService, ISynchronizeContentService
    {
        private ISynchronizationService synchronizationService;

        public SynchronizeContentService() {}

        public SynchronizeContentService(ISynchronizationService _synchronizationService)
        {
            synchronizationService = _synchronizationService;
        }

        public void Initialize(SynchronizeContentConfiguration _synchronizeContentConfiguration)
        {
            synchronizationService = new FtpSynchronizationService(_synchronizeContentConfiguration, new FtpDirectoryService(), new FileDirectoryService(), new CompareDirectoryService());
        }

        public override void InternalExecute()
        {
            synchronizationService.RunSynchronization();
        }
    }
}
