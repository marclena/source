using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vueling.Activities.Contracts.ServiceLibrary.StaticContentHelper;
using Vueling.Activities.Contracts.ServiceLibrary.StyleStats;
using Vueling.Activities.Impl.ServiceLibrary.StaticContentHelper;
using Vueling.Activities.Impl.ServiceLibrary.StyleStats;

namespace Vueling.Activities.Impl.ServiceLibrary.IntegrationTest.Given_StaticContentJS
{
    [TestClass]
    public class When_StaticContentJSResults
    {
        [TestInitialize]
        public void Initialize()
        {
        }
        [TestMethod]
        public void Then_Generate_HelpersImplServiceLibrary()
        {
            string basepathStyleStatsThreshold;

            IStaticContentHelperGenerator staticContentHelperGenerator = new StaticContentHelperGeneratorService();
            string sourceDir = Path.Combine(Directory.GetCurrentDirectory(),
                @"..\..\fake\Vueling.StaticContent.Back.JS.WebUI");
            string targetDir = Path.Combine(Directory.GetCurrentDirectory(),
                @"..\..\fake\Vueling.StaticContent.Back.JS.Helpers.Impl.ServiceLibrary");
            Directory.Delete(targetDir + "\\Helpers", true);
            staticContentHelperGenerator.Initialize(sourceDir, targetDir, "Vueling.StaticContent.Back.JS.Helpers.Impl.ServiceLibrary");
            staticContentHelperGenerator.InternalExecute();
            bool ExistsAllFiles =
                File.Exists(@"..\..\fake\Vueling.StaticContent.Back.JS.Helpers.Impl.ServiceLibrary\Helpers\StaticJS.cs") &&
                File.Exists(@"..\..\fake\Vueling.StaticContent.Back.JS.Helpers.Impl.ServiceLibrary\Helpers\Back.cs") &&
                File.Exists(@"..\..\fake\Vueling.StaticContent.Back.JS.Helpers.Impl.ServiceLibrary\Helpers\back\Js.cs");

            Assert.AreEqual(ExistsAllFiles, true);
        }
    }
}
