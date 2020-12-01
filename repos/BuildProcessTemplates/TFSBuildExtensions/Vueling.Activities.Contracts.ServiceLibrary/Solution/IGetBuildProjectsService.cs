using Microsoft.TeamFoundation.Build.Workflow.Activities;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vueling.Activities.Contracts.ServiceLibrary.Solution
{
    public interface IGetBuildProjectsService
    {
        void Initialize(Workspace workspace, StringList projectsToBuild, string sourcesDirectory);
        void InternalExecute();

        string VersionVisualStudio { get; }

        string[] Projects { get; }
        List<string> Messages { get; }
    }
}
