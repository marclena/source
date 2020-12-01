using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;

namespace Activities.UnitTest.Validation
{
    [TestClass]
    public class FileSizeLimitValidationTest
    {
        [TestMethod]
        public void ValidFileWithCsExtension()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.FileSizeLimitValidation
            {
                Recursively = true,
                RootFolder = @"D:\workspaces\tfs\BuildProcessTemplates\TFSBuildExtensions",
                Size = 700,
            };

            var parameters = new Dictionary<string, object>
            {
                { "PatternFiles", new List<string>{ "*.cs" }},
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke(parameters);

            Assert.AreEqual(false, output["IsExceedingLimit"]);
        }

        [TestMethod]
        public void InValidFileWithCsExtension()
        {
            var target = new TFSBuildExtensions.EnvironmentManagement.Validation.FileSizeLimitValidation
            {
                Recursively = true,
                RootFolder = @"D:\workspaces\tfs\BuildProcessTemplates\TFSBuildExtensions",
                Size = 1,
            };

            var parameters = new Dictionary<string, object>
            {
                { "PatternFiles", new List<string>{ "*.cs" }},
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke(parameters);

            Assert.AreEqual(true, output["IsExceedingLimit"]);
        }
    }
}
