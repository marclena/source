using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using Vueling.Activities.Contracts.ServiceLibrary.Release;
using Vueling.Activities.Impl.ServiceLibrary.Release;

namespace Vueling.Activities.Impl.ServiceLibrary.IntegrationTest.Given_Build_Running
{
    [TestClass]
    public class When_CreateUniqueReleaseItemService_Is_run
    {
        [TestMethod]
        public void Then_ReleaseJsonFileIsGenerated()
        {
            ICreateUniqueReleaseItemService createUniqueReleaseItemService = new CreateUniqueReleaseItemService();

            createUniqueReleaseItemService.Initialize(Directory.GetCurrentDirectory(), "135799", null);

            createUniqueReleaseItemService.InternalExecute();

            Assert.IsTrue(File.Exists(Path.Combine(Directory.GetCurrentDirectory(), Vueling.Activities.Configuration.Configuration.ReleaseFile)));
        }
    }
}
