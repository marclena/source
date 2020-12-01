using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vueling.Activities.Contracts.ServiceLibrary.Release
{
    public interface ICreateUniqueReleaseItemService : IBaseActivityService
    {
        void Initialize(string _binariesDirectory, string _source, Workspace workspace);
    }
}
