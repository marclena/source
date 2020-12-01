using System;

namespace Vueling.Activities.Contracts.ServiceLibrary.TeamFoundationServer
{
    public interface IUpdateFilesFromLocalItemToServerItemService : IBaseActivityService
    {
        void InternalExecute();
    }
}
