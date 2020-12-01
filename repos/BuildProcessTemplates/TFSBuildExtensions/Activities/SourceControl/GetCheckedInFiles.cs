using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.TeamFoundation.Build.Client;
using System.Activities;
using System.ComponentModel;
using Microsoft.TeamFoundation.VersionControl.Client;
using System.IO;
using System.Xml;

namespace TFSBuildExtensions.SourceControl
{
    [BuildActivity(HostEnvironmentOption.All)]
    public class GetCheckedInFiles : BaseCodeActivity
    {
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
        /// Workspace
        /// </summary>        
        [Description("Workspace")]
        [RequiredArgument]
        public InArgument<Workspace> Workspace { get; set; }

        /// <summary>
        /// Result
        /// </summary>        
        [Description("Result")]
        [RequiredArgument]
        public InOutArgument<IList<string>> Result { get; set; }

        [Description("Result server item2")]
        [RequiredArgument]
        public OutArgument<IList<string>> ResultServerItem { get; set; }

        [Description("Shelved files (local item)")]
        [RequiredArgument]
        public OutArgument<List<string>> ShelvedFiles { get; set; }

        /// <summary>
        /// Executes the logic for this workflow activity
        /// </summary>
        protected override void InternalExecute()
        {
            string localPath, file;
            IList<string> checkedInFiles = new List<string>();
            IList<string> serverItemFiles = new List<string>();
            List<string> shelveFiles = new List<string>();

            //Verify if this build is a gated check-in (gated check-in includes shelveset code)
            if (!String.IsNullOrEmpty(this.BuildDetail.ShelvesetName))
            {
                string shelvedName = this.BuildDetail.ShelvesetName.Split(";".ToCharArray())[0];
                string shelvedOwner = this.BuildDetail.ShelvesetName.Split(";".ToCharArray())[1];

                PendingSet[] pendingSets = this.VersionControlServer.Get(this.ActivityContext).QueryShelvedChanges(shelvedName, shelvedOwner);

                //Gets local path for each file
                foreach (PendingSet pendingSet in pendingSets)
                {
                    foreach (PendingChange pendingChange in pendingSet.PendingChanges)
                    {
                        try
                        {
                            if (pendingChange.ChangeType == ChangeType.Delete)
                            {

                                localPath = this.Workspace.Get(this.ActivityContext).TryGetWorkingFolderForServerItem(pendingChange.ServerItem.Substring(0, pendingChange.ServerItem.LastIndexOf("/"))).LocalItem;
                                file = pendingChange.ServerItem.Substring(pendingChange.ServerItem.LastIndexOf("/") + 1, pendingChange.ServerItem.Length - pendingChange.ServerItem.LastIndexOf("/") - 1);

                                AddFile(pendingChange.ServerItem, Path.Combine(localPath, file), checkedInFiles, serverItemFiles);
                            }
                            else
                            {
                                file = this.Workspace.Get(this.ActivityContext).GetWorkingFolderForServerItem(pendingChange.LocalOrServerItem).LocalItem;
                                AddFile(pendingChange.ServerItem, file, checkedInFiles, serverItemFiles);
                                shelveFiles.Add(file);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogBuildWarning("There was an error while getting file mapping from gated chech-in shelve: " + pendingChange.ServerItem + ". " + ex.Message);
                        }
                    }
                }
            }

            if (this.AssociatedChangesets.Get(this.ActivityContext) != null)
            {
                foreach (Changeset changeset in this.AssociatedChangesets.Get(this.ActivityContext))
                {
                    Changeset fullChangeset = this.VersionControlServer.Get(this.ActivityContext).GetChangeset(changeset.ChangesetId);

                    foreach (var changedItem in fullChangeset.Changes)
                    {
                        try
                        {
                            if (changedItem.ChangeType == ChangeType.Delete)
                            {
                                localPath = this.Workspace.Get(this.ActivityContext).TryGetWorkingFolderForServerItem(changedItem.Item.ServerItem.Substring(0, changedItem.Item.ServerItem.LastIndexOf("/"))).LocalItem;
                                file = changedItem.Item.ServerItem.Substring(changedItem.Item.ServerItem.LastIndexOf("/") + 1, changedItem.Item.ServerItem.Length - changedItem.Item.ServerItem.LastIndexOf("/") - 1);
                                AddFile(changedItem.Item.ServerItem, Path.Combine(localPath, file), checkedInFiles, serverItemFiles);
                            }
                            else
                            {
                                file = this.Workspace.Get(this.ActivityContext).GetWorkingFolderForServerItem(changedItem.Item.ServerItem).LocalItem;
                                AddFile(changedItem.Item.ServerItem, file, checkedInFiles, serverItemFiles);
                            }
                        }
                        catch (Exception ex)
                        {
                            LogBuildWarning("There was an error while getting file mapping from checked in changeset: " + changedItem.Item.ServerItem + ". " + ex.Message);
                        }
                    }
                }
            }

            this.Result.Set(this.ActivityContext, checkedInFiles);
            this.ResultServerItem.Set(this.ActivityContext, serverItemFiles);
            this.ShelvedFiles.Set(this.ActivityContext, shelveFiles);
        }

        /// <summary>
        /// Validate that file is correct and add file to checked in files list
        /// </summary>
        /// <param name="checkedInFiles"></param>
        /// <param name="file"></param>
        private void AddFile(string serverItem, string file, IList<string> checkedInFiles, IList<string> serverItemFiles)
        {
            if (ValidateFile(file))
            {
                checkedInFiles.Add(file);
                serverItemFiles.Add(serverItem);
            }
            else
            {
                LogBuildError("File " + file + " could not be validated. Xml is not well-formed.");
            }
        }

        /// <summary>
        /// In case file is an .xml or .config, validates that is an xml well-formed
        /// </summary>
        /// <returns></returns>
        private bool ValidateFile(string file)
        {
            if (File.Exists(file))
            {
                if (Path.GetExtension(file).Equals("xml") || Path.GetExtension(file).Equals("config"))
                {
                    XmlTextReader reader = new XmlTextReader(file);

                    XmlDocument xmlDoc = new XmlDocument();

                    //Load the file into the XmlDocument
                    try
                    {
                        xmlDoc.Load(reader);
                    }
                    catch (System.Xml.XmlException)
                    {
                        return false;
                    }
                    finally
                    {
                        //Close off the connection to the file.
                        reader.Close();
                    }
                }
            }
            else
            {
                LogBuildWarning("File " + file + " does not exist.");
            }
            return true;
        }
    }
}
