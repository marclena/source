using System;
using System.Activities;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMock;

namespace Activities.IntegrationTest.TeamFoundationServer
{
    [TestClass]
    public class GetLastSuccessfulBuildNameFromPRE_isCalled
    {
        private static MockFactory _MockFactory { get; set; }
        private static Mock<IBuildDetail> _buildDetail { get; set; }
        private static Mock<IBuildDefinition> _buildDefinition { get; set; }
        private static Mock<IBuildServer> _buildServer { get; set; }

        [TestMethod]
        public void ReturnBuildWithVuelingXXXPRO64()
        {
            _MockFactory = new MockFactory();
            _buildServer = _MockFactory.CreateMock<IBuildServer>();
            _buildDefinition = _MockFactory.CreateMock<IBuildDefinition>();
            _buildDetail = _MockFactory.CreateMock<IBuildDetail>();

            _buildDetail.Expects.AtLeastOne.GetProperty(x => x.BuildDefinition).WillReturn(_buildDefinition.MockObject);
            _buildDefinition.Expects.AtLeastOne.GetProperty(x => x.Name).WillReturn("Vueling.XXX.PRO.64");
            _buildDetail.Expects.AtLeastOne.Method(x => x.Save());
            _buildDetail.Expects.AtLeastOne.GetProperty(x => x.DropLocation).WillReturn(@"D:\workspaces\tfs\Vueling.Configuration\PRE\Vueling.Configuration.Library\bin\Debug");

            var target = new TFSBuildExtensions.TeamFoundationServer.GetLastSuccessfulBuildNameFromPRE {};

            var invoker = new WorkflowInvoker(target);

            invoker.Extensions.Add(_buildServer.MockObject);
            invoker.Extensions.Add(_buildDefinition.MockObject);
            invoker.Extensions.Add(_buildDetail.MockObject);

            var output = invoker.Invoke();

            Assert.IsNotNull(output["LastSuccessfulBuildName"]);
        }

        [TestMethod]
        public void ReturnBuildWithVuelingRemoINTTEST()
        {
            _MockFactory = new MockFactory();
            _buildServer = _MockFactory.CreateMock<IBuildServer>();
            _buildDefinition = _MockFactory.CreateMock<IBuildDefinition>();
            _buildDetail = _MockFactory.CreateMock<IBuildDetail>();

            _buildDetail.Expects.AtLeastOne.GetProperty(x => x.BuildDefinition).WillReturn(_buildDefinition.MockObject);
            _buildDefinition.Expects.AtLeastOne.GetProperty(x => x.Name).WillReturn("Vueling.Remo.INT.TEST.64");
            _buildDetail.Expects.AtLeastOne.Method(x => x.Save());
            _buildDetail.Expects.AtLeastOne.GetProperty(x => x.DropLocation).WillReturn(@"D:\workspaces\tfs\Vueling.Configuration\PRE\Vueling.Configuration.Library\bin\Debug");

            var target = new TFSBuildExtensions.TeamFoundationServer.GetLastSuccessfulBuildNameFromPRE { };

            var invoker = new WorkflowInvoker(target);

            invoker.Extensions.Add(_buildServer.MockObject);
            invoker.Extensions.Add(_buildDefinition.MockObject);
            invoker.Extensions.Add(_buildDetail.MockObject);

            var output = invoker.Invoke();

            Assert.IsNotNull(output["LastSuccessfulBuildName"]);
        }
    }
}
