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
    public class When_TargetDirectory_DoesntExist
    {
        [TestInitialize]
        public void Initialize()
        {
        }
        [TestMethod]
        public void Then_DoNothing()
        {
            string basepathStyleStatsThreshold;

            IStaticContentHelperGenerator staticContentHelperGenerator = new StaticContentHelperGeneratorService();
            string sourceDir = Path.Combine(Directory.GetCurrentDirectory(),
                @"..\..\fake2\Vueling.StaticContent.Back.JS.WebUI");
            string targetDir = Path.Combine(Directory.GetCurrentDirectory(),
                @"..\..\fake2\Vueling.StaticContent.Back.JS.Helpers.Impl.ServiceLibrary");
            
            staticContentHelperGenerator.Initialize(sourceDir, targetDir, "Vueling.StaticContent.Back.JS.Helpers.Impl.ServiceLibrary");
            staticContentHelperGenerator.InternalExecute();
            

            Assert.AreEqual(Directory.Exists(targetDir + "\\Helpers"), false);
        }
    }
}
