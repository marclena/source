using System;
using System.Activities;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Activities.UnitTest.TeamFoundationServer
{
    [TestClass]
    public class When_GetProjectWorkItemsFromId_isCalled
    {
        [TestMethod]
        public void Then_List_of_Project_WorkItems_Is_Received()
        {
            List<string> _mockedIssueKey = new List<string>();
            _mockedIssueKey.Add("TECH-200");

            var target = new TFSBuildExtensions.TeamFoundationServer.GetProjectWorkItemsFromId
            {
                WorkItemChildId = 14703
            };

            var parameters = new Dictionary<string, object>
            { { "JiraIssueKeyFromProjects", new List<string>() }
            }; 
            
            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke(parameters);

            CollectionAssert.AreEquivalent(_mockedIssueKey, (List<string>)output["JiraIssueKeyFromProjects"]);
        }
    }
}
