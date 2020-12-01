using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Build.Client;
using System.ComponentModel;
using System.Activities;
using System.IO;
using Microsoft.TeamFoundation.Build.Workflow.Services;
using TFSBuildExtensions.Library.CodeMetrics.Report;
using System.Globalization;
using System.Diagnostics;
using TFSBuildExtensions.Library.CodeMetrics.Threshold;
using Microsoft.TeamFoundation.VersionControl.Client;

namespace TFSBuildExtensions.SourceControl
{    
    [BuildActivity(HostEnvironmentOption.All)]
    public class FilesInChangesetActivity : BaseCodeActivity
    {
        /// <summary>
        /// Extensions
        /// </summary>
        [Description("Extensions")]
        [RequiredArgument]
        public InArgument<string[]> Extensions { get; set; }

        /// <summary>
        /// AssociatedChangesets
        /// </summary>
        [Description("AssociatedChangesets")]
        [RequiredArgument]
        public InArgument<IList<Changeset>> AssociatedChangesets { get; set; }

        /// <summary>
        /// VersionControlServer
        /// </summary>
        [Description("VersionControlServer")]
        [RequiredArgument]
        public InArgument<VersionControlServer> VersionControlServer { get; set; }

        /// <summary>
        /// Result
        /// </summary>        
        [Description("Properties")]
        [RequiredArgument]
        public InOutArgument<bool> Result { get; set; }

        /// <summary>
        /// Executes the logic for this workflow activity
        /// </summary>
        protected override void InternalExecute()
        {
            bool result = false;

            if (this.Extensions.Get(this.ActivityContext) != null && !String.IsNullOrEmpty(this.BuildDetail.ShelvesetName))
            {
                string shelvedName = this.BuildDetail.ShelvesetName.Split(";".ToCharArray())[0];
                string shelvedOwner = this.BuildDetail.ShelvesetName.Split(";".ToCharArray())[1];

                PendingSet[] pendingSets = this.VersionControlServer.Get(this.ActivityContext).QueryShelvedChanges(shelvedName, shelvedOwner);

                foreach (PendingSet pendingSet in pendingSets)
                {
                    foreach (PendingChange pendingChange in pendingSet.PendingChanges)
                    {
                        string extension = Path.GetExtension(pendingChange.FileName);
                        if (this.Extensions.Get(this.ActivityContext).Contains(extension))
                        {
                            result = true;
                            break;
                        }
                    }
                    if (result)
                        break;
                }
            }

            if (this.Extensions.Get(this.ActivityContext) != null && this.AssociatedChangesets.Get(this.ActivityContext) != null)
            {
                foreach (Changeset changeset in this.AssociatedChangesets.Get(this.ActivityContext))
                {
                    Changeset fullChangeset = this.VersionControlServer.Get(this.ActivityContext).GetChangeset(changeset.ChangesetId);

                    foreach (var changedItem in fullChangeset.Changes)
                    {
                        string item = changedItem.Item.ServerItem;
                        string extension = Path.GetExtension(item);
                        if (this.Extensions.Get(this.ActivityContext).Contains(extension))
                        {
                            result = true;
                            break;
                        }
                    }
                    if (result)
                        break;
                }
            }

            this.Result.Set(this.ActivityContext, result);
        }

        private void PartiallyFailCurrentBuild()
        {
            this.BuildDetail.Status = BuildStatus.PartiallySucceeded;
            this.BuildDetail.Save();
        }

        private void FailCurrentBuild()
        {
            this.BuildDetail.Status = BuildStatus.Failed;
            this.BuildDetail.Save();
        }
    }
}
