using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Vueling.Activities.Contracts.ServiceLibrary.TeamFoundationServer;
using Vueling.Extensions.Library.DI;

namespace Vueling.Activities.Impl.ServiceLibrary.TeamFoundationServer
{
    [RegisterServiceAttribute]
    public class UpdateFilesFromLocalItemToServerItemService : BaseActivityService, IUpdateFilesFromLocalItemToServerItemService
    {
        private List<string> sourceFiles, targetServerItems;
        private Workspace workspace;

        public UpdateFilesFromLocalItemToServerItemService(List<string> _sourceFiles, Workspace _workspace, List<string> _targetServerItems)
        {
            sourceFiles = _sourceFiles;
            targetServerItems = _targetServerItems;
            workspace = _workspace;
        }

        public override void InternalExecute()
        {
            int numFile = 0;
            ItemSpec[] itemSpecList = new ItemSpec[targetServerItems.Count];
            List<string> checkinComment = new List<string>();

            var fileName = ".\\UpdateFilesFromLocalItemToServerItem.log";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
                fileName = ".\\UpdateFilesFromLocalItemToServerItem.log";
            }

            using (StreamWriter writer = new StreamWriter(new FileStream(fileName, FileMode.Append)))
            {
                writer.WriteLine("{0} - START PROCESS UpdateFilesFromLocalItemToServerItem", DateTime.Now);
                writer.WriteLine("--------------------------------------------------------------------------------------------------------------------------------");
                writer.WriteLine("");
            }

            foreach (var TargetServerItem in targetServerItems)
            {
                ItemSpec itemSpecBranchBuild = new ItemSpec(TargetServerItem, RecursionType.Full);
                itemSpecList[numFile] = itemSpecBranchBuild;
                workspace.Get(new GetRequest(itemSpecBranchBuild, VersionSpec.Latest), GetOptions.None);

                workspace.PendEdit(TargetServerItem, RecursionType.Full);

                //copy
                string localTargetServerItem = workspace.GetLocalItemForServerItem(TargetServerItem);
                File.Copy(sourceFiles[numFile], localTargetServerItem, true);

                using (StreamWriter writer = new StreamWriter(new FileStream(fileName, FileMode.Append)))
                {
                    writer.WriteLine("COPYING FILE:");
                    writer.WriteLine("<{0}>", sourceFiles[numFile]);
                    writer.WriteLine("TO");
                    writer.WriteLine("<{0}>", localTargetServerItem);
                    writer.WriteLine("");
                }

                checkinComment.Add("\"" + TargetServerItem + "\"");
                numFile++;
            }

            //checkout            
            var pendingChanges = workspace.GetPendingChanges(itemSpecList);

            //checkin                    
            if (pendingChanges.Any())
            {
                WorkspaceCheckInParameters parameters = new WorkspaceCheckInParameters(pendingChanges, "Automatic updated files: " + string.Join(",", checkinComment.ToArray()))
                {
                    OverrideGatedCheckIn = true,
                };
                workspace.CheckIn(parameters);
                using (StreamWriter writer = new StreamWriter(new FileStream(fileName, FileMode.Append)))
                {
                    writer.WriteLine("CHECKIN FILES:");
                    writer.WriteLine("{0}", string.Join(",", checkinComment.ToArray()));
                    writer.WriteLine("");
                }
            }
            else
            {
                using (StreamWriter writer = new StreamWriter(new FileStream(fileName, FileMode.Append)))
                {
                    writer.WriteLine("NO PENDING FILES, NO CHECKIN");
                    writer.WriteLine("");
                }
            }
            using (StreamWriter writer = new StreamWriter(new FileStream(fileName, FileMode.Append)))
            {
                writer.WriteLine("--------------------------------------------------------------------------------------------------------------------------------");
                writer.WriteLine("{0} - END PROCESS UpdateFilesFromLocalItemToServerItem", DateTime.Now);
            }
        }
    }
}
