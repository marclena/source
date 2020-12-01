using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

namespace TFSBuildExtensions.TeamFoundationServer
{
    [BuildActivity(HostEnvironmentOption.All)]
    public class GetProjectWorkItemsFromId : BaseCodeActivity
    {
        [Description("WorkItem child from which find workitem Projects parents")]
        [RequiredArgument]
        public InArgument<int> WorkItemChildId { get; set; }

        [Description("List of associated Project WorkItems")]
        public InOutArgument<List<int>> ParentProjectWorkItems { get; set; }

        [Description("List of Jira issue key of afected projects")]
        public InOutArgument<List<string>> JiraIssueKeyFromProjects { get; set; }

        private TFSBuildExtensions.TeamFoundationServer.TeamFoundationServerConnection tfsInstance;
        private WorkItemStore _workItemStore;
        private Project _project;
        private List<int> _parentProjectWorkItems;
        private List<string> _jiraIssueKeyFromProjects;
        WorkItemLinkTypeEnd parentLinkTypeEnd;
        private int levelsRecursion;


        protected override void InternalExecute()
        {
            tfsInstance = new TFSBuildExtensions.TeamFoundationServer.TeamFoundationServerConnection();
            AccessWorkItemStoreProject();
            getParentProjectWorkItems();
        }

        private void AccessWorkItemStoreProject()
        {
            if (_project == null)
            {
                _workItemStore = tfsInstance.Server.GetService<WorkItemStore>();

                _project = _workItemStore.Projects[tfsInstance.TeamProject];
            }
        }

        private void getParentProjectWorkItems()
        {
            _jiraIssueKeyFromProjects = this.JiraIssueKeyFromProjects.Get(this.ActivityContext);

            _parentProjectWorkItems = new List<int>();
            parentLinkTypeEnd = _workItemStore.WorkItemLinkTypes.LinkTypeEnds["Parent"];
            WorkItem workItem = _workItemStore.GetWorkItem(this.WorkItemChildId.Get(this.ActivityContext));

            AddRecursiveParentWorkItems(workItem);

            this.ParentProjectWorkItems.Set(this.ActivityContext, _parentProjectWorkItems);
            this.JiraIssueKeyFromProjects.Set(this.ActivityContext, _jiraIssueKeyFromProjects);
        }

        private void AddRecursiveParentWorkItems(WorkItem workItem)
        {
            levelsRecursion++;

            foreach (WorkItemLink workItemLink in workItem.WorkItemLinks)
            {
                AddIdToCollection(workItemLink);
            }
        }

        private void AddIdToCollection(WorkItemLink workItemLink)
        {
            WorkItem workitem = IsLinkedProjectWorkItem(workItemLink);
            if (workitem != null)
            {
                if (!_parentProjectWorkItems.Contains(workItemLink.TargetId))
                {
                    _parentProjectWorkItems.Add(workItemLink.TargetId);
                    _jiraIssueKeyFromProjects.Add(workitem.Fields["Vueling.Project"].Value.ToString());
                }
            }
            else
            {
                if (levelsRecursion < 5)
                {
                    AddRecursiveParentWorkItems(_workItemStore.GetWorkItem(workItemLink.TargetId));
                }
            }
        }

        private WorkItem IsLinkedProjectWorkItem(WorkItemLink workItemLink)
        {
            WorkItem linkedWorkItem = _workItemStore.GetWorkItem(workItemLink.TargetId);

            if (linkedWorkItem.Fields["System.WorkItemType"].Value.Equals("Project"))
            {
                return linkedWorkItem;
            }
            else
            {
                return null;
            }
        }
    }
}
