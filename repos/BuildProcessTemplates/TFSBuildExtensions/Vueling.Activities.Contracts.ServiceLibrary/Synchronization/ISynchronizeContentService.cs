using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSBuildExtensions.Library.SynchronizeContent;

namespace Vueling.Activities.Contracts.ServiceLibrary.Synchronization
{
    public interface ISynchronizeContentService : IBaseActivityService
    {
        void Initialize(SynchronizeContentConfiguration synchronizeContentConfiguration);
    }
}