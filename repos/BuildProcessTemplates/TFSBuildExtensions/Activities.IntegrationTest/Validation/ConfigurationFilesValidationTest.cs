using System;
using System.Activities;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Activities.IntegrationTest.Validation
{
    [TestClass]
    public class ConfigurationFilesValidationTest
    {
        [TestMethod]
        public void ListFilesValidationOk()
        {
            List<string> changesetFiles = new List<string>();

            changesetFiles.Add(@"D:\workspaces\tfs\Vueling.Configuration\PRO\Vueling.Configuration.Config\Vueling.BolsaTrabajo.WebUI\app.config");
            changesetFiles.Add(@"D:\Builds\83\Vueling\Vueling.Configuration.PRO.BACKSERVERS.64\Sources\BuildProcessTemplates\TFSBuildExtensions\Vueling.Minifier.ConsoleUI\JsCssMin.cs");
            changesetFiles.Add(@"D:\Builds\83\Vueling\Vueling.Configuration.PRO.BACKSERVERS.64\Sources\BuildProcessTemplates\TFSBuildExtensions\Vueling.Minifier.ConsoleUI\Options.cs");
            changesetFiles.Add(@"D:\Builds\83\Vueling\Vueling.Configuration.PRO.BACKSERVERS.64\Sources\BuildProcessTemplates\TFSBuildExtensions\Vueling.Minifier.ConsoleUI\packages.config");
            changesetFiles.Add(@"D:\Builds\83\Vueling\Vueling.Configuration.PRO.BACKSERVERS.64\Sources\BuildProcessTemplates\TFSBuildExtensions\Vueling.Minifier.ConsoleUI\Program.cs");
            changesetFiles.Add(@"D:\workspaces\tfs\Vueling.Configuration\PRO\Vueling.Configuration.Config\Vueling.Account.Publisher.WCF.WebService.IntegrationTest\iom.xml");
            changesetFiles.Add(@"D:\workspaces\tfs\Vueling.Configuration\PRO\Vueling.Configuration.Config\Vueling.TfsOperations.WCF.WebService\app.config");
            changesetFiles.Add(@"D:\workspaces\tfs\Vueling.Configuration\PRO\Vueling.Configuration.Config\Vueling.TfsOperations.WCF.WebService\iom.xml");
            changesetFiles.Add(@"D:\workspaces\tfs\Vueling.Configuration\PRO\Vueling.Configuration.Config\Vueling.TfsOperations.ServiceLibrary\app.config");
            changesetFiles.Add(@"D:\workspaces\tfs\Vueling.Configuration\PRO\Vueling.Configuration.Config\Vueling.TfsOperations.ServiceLibrary\iom.xml");
            changesetFiles.Add(@"D:\workspaces\tfs\Vueling.Configuration\PRO\Vueling.Configuration.Config\global\connectionStrings.xml");

            var target = new TFSBuildExtensions.Validation.ConfigurationFilesValidation
            {
                Environment = "INT"
            };

            var parameters = new Dictionary<string, object>
            {
                { "FilesToProcess", changesetFiles }
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke(parameters);

            Assert.AreEqual("0",((List<string>)output["Errors"]).Count.ToString());
            Assert.AreEqual("3", ((List<string>)output["IOMs"]).Count.ToString());
            Assert.AreEqual("5", ((List<string>)output["CheckedInFilesToDeploy"]).Count.ToString());
            Assert.IsTrue((bool)output["UploadedFilesNeedRunAutoSetupEnvironment"]);
            Assert.IsTrue((bool)output["ContainsGlobalOrUsers"]);
        }
    }
}
