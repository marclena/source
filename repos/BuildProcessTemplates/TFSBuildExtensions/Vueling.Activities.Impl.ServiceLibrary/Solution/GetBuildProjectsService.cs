using Microsoft.TeamFoundation.Build.Workflow.Activities;
using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Vueling.Activities.Contracts.ServiceLibrary.Solution;
using Vueling.Extensions.Library.DI;

namespace Vueling.Activities.Impl.ServiceLibrary.Solution
{
    [RegisterServiceAttribute]
    public class GetBuildProjectsService : IGetBuildProjectsService
    {
        private static string[] PROJECT_TYPES = new string[] 
        {
            ".csproj",
            ".fsproj",
            ".vbproj",
            ".modelproj",
            ".ccproj",
            ".dbproj",
            ".sqlproj",
            ".vcxproj",
            ".vcproj",
            ".dsp",
            ".mdp",
            ".props",
            ".vcp",
            ".vdproj",
            ".vdp",
            ".vbp"
        };

        private Workspace workspace;
        private string sourcesDirectory;
        private StringList projectsToBuild;
        private string versionVisualStudio;
        private string[] projects;
        private List<string> messages = new List<string>();

        public void Initialize(Workspace _workspace, StringList _projectsToBuild, string _sourcesDirectory)
        {
            workspace = _workspace;
            projectsToBuild = _projectsToBuild;
            sourcesDirectory = _sourcesDirectory;

            if (workspace == null && sourcesDirectory == null)
            {
                throw new Exception("One of this arguments is required: Workspace or SourcesDirectory");
            }
        }

        public void InternalExecute()
        {
            List<string> _projects = new List<string>();

            foreach (string item in projectsToBuild)
            {
                if (PROJECT_TYPES.Contains(Path.GetExtension(item)))
                {
                    _projects.Add(Path.GetFileNameWithoutExtension(item));
                    messages.Add("Adding " + Path.GetFileNameWithoutExtension(item) + " project");
                }
                else if (Path.GetExtension(item) == ".sln")
                {
                    getProjectsFromSolutionFile(_projects, item);
                }
                else
                    messages.Add("ERROR: Item '" + item + "' unkown");
            }

            projects = _projects.ToArray();
        }

        private void getProjectsFromSolutionFile(List<string> _projects, string item)
        {
            string file;

            if (workspace == null)
            {
                file = Path.Combine(sourcesDirectory, Path.GetFileName(item));
            }
            else
            {
                file = workspace.GetLocalItemForServerItem(item);
            }

            GetVersionVisualStudio(file);
            _projects.AddRange(this.GetSolutionProjects(file));
        }

        private void GetVersionVisualStudio(string file)
        {
            List<string> lines = File.ReadLines(file).ToList();

            foreach (var line in lines)
            {
                if (line.StartsWith("# Visual Studio"))
                {
                    versionVisualStudio = line.Substring(line.LastIndexOf(" "));
                    messages.Add("Solution created with Visual Studio " + line.Substring(line.LastIndexOf(" ")));
                    break;
                }
            }
        }

        private List<string> GetSolutionProjects(string file)
        {
            List<string> projects = new List<string>();

            string content = File.ReadAllText(file);

            string regexPattern = @"Project\(""{.*?\""\) = "".*?"", "".*?""";

            foreach (Match m in Regex.Matches(content, regexPattern))
            {
                string project = m.Value.Split('=')[1];
                project = project.Split(',')[1];
                project = project.Trim().Substring(1, project.Length - 3);
                project = System.IO.Path.GetFileName(project);

                if (PROJECT_TYPES.Contains(Path.GetExtension(project)))
                {
                    projects.Add(Path.GetFileNameWithoutExtension(project));
                    messages.Add("Adding " + Path.GetFileNameWithoutExtension(project) + " project");
                }
            }

            return projects;
        }

        public string VersionVisualStudio
        {
            get { return versionVisualStudio; }
        }

        public string[] Projects
        {
            get { return projects; }
        }

        public List<string> Messages
        {
            get { return messages; }
        }
    }
}
