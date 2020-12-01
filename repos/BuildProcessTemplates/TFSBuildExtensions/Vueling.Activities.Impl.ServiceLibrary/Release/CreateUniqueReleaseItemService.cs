using Microsoft.TeamFoundation.VersionControl.Client;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TFSBuildExtensions.Library.Release;
using Vueling.Activities.Contracts.ServiceLibrary.Release;
using Vueling.Extensions.Library.DI;

namespace Vueling.Activities.Impl.ServiceLibrary.Release
{
    [RegisterServiceAttribute]
    public class CreateUniqueReleaseItemService : BaseActivityService, ICreateUniqueReleaseItemService
    {
        private string binariesDirectory;
        private string source;
        private Workspace workspace;

        private ReleaseUniqueIdentifier releaseUniqueIdentifier;

        public void Initialize(string _binariesDirectory, string _source, Workspace _workspace)
        {
            binariesDirectory = _binariesDirectory;
            source = _source;
            workspace = _workspace;
        }

        public override void InternalExecute()
        {
            var releasePath = Path.Combine(binariesDirectory, Configuration.Configuration.ReleaseFile);

            if (!File.Exists(releasePath))
            {
                releaseUniqueIdentifier = new ReleaseUniqueIdentifier();

                releaseUniqueIdentifier.guid = Guid.NewGuid();
                releaseUniqueIdentifier.source = source;

                releaseUniqueIdentifier.ServerItems = new List<string>();
                WorkingFolder[] workingFolders = workspace.Folders;
                foreach (var workingFolder in workingFolders)
                {
                    releaseUniqueIdentifier.ServerItems.Add(workingFolder.ServerItem);
                }

                using (StreamWriter sw = new StreamWriter(releasePath))
                {
                    sw.Write(JsonConvert.SerializeObject(releaseUniqueIdentifier));
                }
            }
        }
    }
}
