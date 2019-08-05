using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;
using System.Collections.Generic;
using NMock;
using Microsoft.TeamFoundation.Build.Client;

namespace Activities.IntegrationTest.AWS
{
    [TestClass]
    public class When_UploadAWSPackageToS3
    {
        private static MockFactory _MockFactory { get; set; }
        private static Mock<IBuildDetail> _buildDetail { get; set; }
        private static Mock<IBuildDefinition> _buildDefinition { get; set; }

        [TestInitialize]
        public void Init()
        {
            _MockFactory = new MockFactory();
            _buildDefinition = _MockFactory.CreateMock<IBuildDefinition>();
            _buildDetail = _MockFactory.CreateMock<IBuildDetail>();
        }
        
        [TestMethod]
        public void Then_DeployPackageVuelingXXXPackageIsCreated()
        {
            _buildDetail.Expects.AtLeastOne.GetProperty(x => x.BuildNumber).WillReturn("Vueling.XXX.INT.64_20150311.1");
            _buildDetail.Expects.AtLeastOne.GetProperty(x => x.BuildDefinition).WillReturn(_buildDefinition.MockObject);
            _buildDefinition.Expects.AtLeastOne.GetProperty(x => x.Name).WillReturn("Vueling.XXX.INT.64");

            var target = new TFSBuildExtensions.AWS.UploadAWSPackageToS3
            {
                BinariesDirectory = @"\\wbcnvuebld\Drops\Vueling.XXX.INT.64\Vueling.XXX.INT.64_20150311.1",
               WebSite = "Default Web Site"
            };

            var parameters = new Dictionary<string, object> 
            { { "ProjectsToIncludeInPackage", new List<string> {"Vueling.XXX.Subscriber.WindowsService", "Vueling.XXX.WebUI", "Vueling.XXX.WCF.WebService", "Vueling.XXX.WCF.REST.WebService", "Vueling_XXX.Database"} } 
            };

            var invoker = new WorkflowInvoker(target);

            invoker.Extensions.Add(_buildDefinition.MockObject);
            invoker.Extensions.Add(_buildDetail.MockObject);

            var output = invoker.Invoke(parameters);
        }

        [TestMethod]
        public void Then_DeployPackageSkySalesIsCreated()
        {
            _buildDetail.Expects.AtLeastOne.GetProperty(x => x.BuildNumber).WillReturn("Vueling.XXX.INT.64_20141126.4");
            _buildDetail.Expects.AtLeastOne.GetProperty(x => x.BuildDefinition).WillReturn(_buildDefinition.MockObject);
            _buildDefinition.Expects.AtLeastOne.GetProperty(x => x.Name).WillReturn("SKYSALES.INT.64");

            var target = new TFSBuildExtensions.AWS.UploadAWSPackageToS3
            {
                BinariesDirectory = @"\\WBCNVUEBLD\Drops\SKYSALES.INT.64\SKYSALES.INT.64_20141209.6",
                WebSite = "SkySales"
            };

            var parameters = new Dictionary<string, object> 
            { { "ProjectsToIncludeInPackage", new List<string> {"SkySales"} } 
            };

            var invoker = new WorkflowInvoker(target);

            invoker.Extensions.Add(_buildDefinition.MockObject);
            invoker.Extensions.Add(_buildDetail.MockObject);

            var output = invoker.Invoke(parameters);            
        }

        [TestMethod]
        public void Then_DeployPackageSkysalesIsCreatedLocalEnvironment()
        {
            _buildDetail.Expects.AtLeastOne.GetProperty(x => x.BuildNumber).WillReturn("SKYSALES.INT.64_20141209.9");
            _buildDetail.Expects.AtLeastOne.GetProperty(x => x.BuildDefinition).WillReturn(_buildDefinition.MockObject);
            _buildDefinition.Expects.AtLeastOne.GetProperty(x => x.Name).WillReturn("SKYSALES.INT.64");

            var target = new TFSBuildExtensions.AWS.UploadAWSPackageToS3
            {
                BinariesDirectory = @"C:\Temp\SKYSALES.INT.64_20141209.9",
                WebSite = "SkySales"
            };

            var parameters = new Dictionary<string, object> 
            { { "ProjectsToIncludeInPackage", new List<string> {"SkySales"} } 
            };

            var invoker = new WorkflowInvoker(target);

            invoker.Extensions.Add(_buildDefinition.MockObject);
            invoker.Extensions.Add(_buildDetail.MockObject);

            var output = invoker.Invoke(parameters);            
        }

        [TestMethod]
        public void Then_DeployPackageCMSIsCreated()
        {
            _buildDetail.Expects.AtLeastOne.GetProperty(x => x.BuildNumber).WillReturn("Vueling.Corporative.FRONT.PRE.64_20141202.1");
            _buildDetail.Expects.AtLeastOne.GetProperty(x => x.BuildDefinition).WillReturn(_buildDefinition.MockObject);
            _buildDefinition.Expects.AtLeastOne.GetProperty(x => x.Name).WillReturn("Vueling.Corporative.FRONT.PRE.64");

            var target = new TFSBuildExtensions.AWS.UploadAWSPackageToS3
            {
                BinariesDirectory = @"\\wbcnvuebld\Drops\Vueling.Corporative.FRONT.PRE.64\Vueling.Corporative.FRONT.PRE.64_20141202.1",
                WebSite = "Vueling.Corporative"
            };

            var parameters = new Dictionary<string, object> 
            { { "ProjectsToIncludeInPackage", new List<string> {"Vueling.Corporative.WebUI"} } 
            };

            var invoker = new WorkflowInvoker(target);

            invoker.Extensions.Add(_buildDefinition.MockObject);
            invoker.Extensions.Add(_buildDetail.MockObject);

            var output = invoker.Invoke(parameters);
        }

        [TestMethod]
        public void Then_DeployPackageCMSIsCreatedInLocalEnvironment()
        {
            _buildDetail.Expects.AtLeastOne.GetProperty(x => x.BuildNumber).WillReturn("Vueling.Corporative.FRONT.PRE.64_20141202.1");
            _buildDetail.Expects.AtLeastOne.GetProperty(x => x.BuildDefinition).WillReturn(_buildDefinition.MockObject);
            _buildDefinition.Expects.AtLeastOne.GetProperty(x => x.Name).WillReturn("Vueling.Corporative.FRONT.PRE.64");

            var target = new TFSBuildExtensions.AWS.UploadAWSPackageToS3
            {
                BinariesDirectory = @"C:\Temp\Vueling.Corporative.FRONT.PRE.64_20141202.1",
                WebSite = "SkySales"
            };

            var parameters = new Dictionary<string, object> 
            { { "ProjectsToIncludeInPackage", new List<string> {"SkySales"} } 
            };

            var invoker = new WorkflowInvoker(target);

            invoker.Extensions.Add(_buildDefinition.MockObject);
            invoker.Extensions.Add(_buildDetail.MockObject);

            var output = invoker.Invoke(parameters);
        }

        [TestMethod]
        public void Then_DeployPackageConfigurationIsCreated()
        {
            _buildDetail.Expects.AtLeastOne.GetProperty(x => x.BuildNumber).WillReturn("Vueling.Configuration.PRE.64_20150210.1");
            _buildDetail.Expects.AtLeastOne.GetProperty(x => x.BuildDefinition).WillReturn(_buildDefinition.MockObject);
            _buildDefinition.Expects.AtLeastOne.GetProperty(x => x.Name).WillReturn("Vueling.Configuration.PRE.64");

            var target = new TFSBuildExtensions.AWS.UploadAWSPackageToS3
            {
                BinariesDirectory = @"\\wbcnvuebld\Builds\85\Vueling\Vueling.Configuration.PRE.64\Binaries",
                WebSite = ""
            };

            var parameters = new Dictionary<string, object> 
            { { "ProjectsToIncludeInPackage", new List<string> {"Vueling.Configuration.Config"} } 
            };

            var invoker = new WorkflowInvoker(target);

            invoker.Extensions.Add(_buildDefinition.MockObject);
            invoker.Extensions.Add(_buildDetail.MockObject);

            var output = invoker.Invoke(parameters);
        }

        [TestMethod]
        public void Then_DeployPackageStaticContentIsCreated()
        {
            _buildDetail.Expects.AtLeastOne.GetProperty(x => x.BuildNumber).WillReturn("Vueling.StaticContentSkySales.PRE.64_20150210.1");
            _buildDetail.Expects.AtLeastOne.GetProperty(x => x.BuildDefinition).WillReturn(_buildDefinition.MockObject);
            _buildDefinition.Expects.AtLeastOne.GetProperty(x => x.Name).WillReturn("Vueling.StaticContentSkySales.PRE.64");

            var target = new TFSBuildExtensions.AWS.UploadAWSPackageToS3
            {
                BinariesDirectory = @"\\wbcnvuebld4\Builds\8\Vueling\Vueling.StaticContentSkySales.PRE.64\Binaries",
                WebSite = ""
            };

            var parameters = new Dictionary<string, object> 
            { { "ProjectsToIncludeInPackage", new List<string> {"Vueling.StaticContent.WebUI"} } 
            };

            var invoker = new WorkflowInvoker(target);

            invoker.Extensions.Add(_buildDefinition.MockObject);
            invoker.Extensions.Add(_buildDetail.MockObject);

            var output = invoker.Invoke(parameters);
        }

    }
}
