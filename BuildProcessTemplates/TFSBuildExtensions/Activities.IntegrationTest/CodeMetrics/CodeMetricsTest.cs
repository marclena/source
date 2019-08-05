using System;
using System.Activities;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.TeamFoundation.Build.Client;
using Microsoft.TeamFoundation.Build.Workflow.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NMock;

namespace Activities.UnitTest.CodeMetrics
{
    [TestClass]
    public class CodeMetricsTest
    {
        private static MockFactory _MockFactory { get; set; }
        private static Mock<IBuildDetail> _buildDetail { get; set; }

        [TestMethod]
        public void CodeMetricsVuelingConfigurationLibrary()
        {
            var target = new TFSBuildExtensions.CodeMetrics.CodeMetricsActivity
            {
                BinariesDirectory = @"c:\workspaces\tfs\Vueling\Vueling.Configuration\PRE\Vueling.Configuration.Library\bin\Debug",
                OutputFile = @"c:\workspaces\tfs\Vueling\Vueling.Configuration\PRE\Vueling.Configuration.Library\bin\Debug\Vueling.Configuration.INT.64_20130527.3.log",
                ThresholdFile = @"c:\workspaces\tfs\Vueling\BuildProcessTemplates\TFSBuildExtensions\SupportFiles\CodeMetricsThreshold.xml",
            };

            var parameters = new Dictionary<string, object> 
            { { "FilesToProcess", new string[] {"Vueling.Extensions.Library.dll"} } 
            };

            var invoker = new WorkflowInvoker(target);

            _MockFactory = new MockFactory();
            _buildDetail = _MockFactory.CreateMock<IBuildDetail>();

            _buildDetail.IgnoreUnexpectedInvocations = true;
            _buildDetail.Expects.AtLeastOne.GetProperty<BuildStatus>(x => x.Status).WillReturn(BuildStatus.InProgress);
            _buildDetail.Expects.AtLeastOne.Method(x => x.Save());
            _buildDetail.Expects.AtLeastOne.GetProperty(x => x.DropLocation).WillReturn(@"c:\workspaces\Vueling\tfs\Vueling.Configuration\PRE\Vueling.Configuration.Library\bin\Debug");

            invoker.Extensions.Add(_buildDetail.MockObject);

            try
            {
                var output = invoker.Invoke(parameters);
            }
            catch (Exception exception)
            {

                throw exception;
            }

            Assert.IsNotNull(target);            
        }

        [TestMethod]
        public void CodeMetricsVuelingXXX()
        {
            var target = new TFSBuildExtensions.CodeMetrics.CodeMetricsActivity
            {
                BinariesDirectory = @"D:\Temp\Vueling.XXX.PRE.64\Binaries",
                OutputFile = @"D:\Temp\Vueling.XXX.PRE.64\Binaries\Vueling.XXX.PRE.64_20130423.5.log",
                ThresholdFile = @"D:\workspaces\tfs\BuildProcessTemplates\TFSBuildExtensions\SupportFiles\CodeMetricsThreshold.xml",
            };

            var parameters = new Dictionary<string, object> 
            { { "FilesToProcess", new string[] {"Vueling.XXX.WebUI.dll", "Vueling.XXX.WCF.WebService.dll", "Vueling.XXX.WebUI.UnitTest.dll", "Vueling.XXX.WCF.WebService.UnitTest.dll", "Vueling.XXX.WCF.REST.WebService.dll", "Vueling.XXX.WCF.REST.WebService.UnitTest.dll", "Vueling.XXX.Contracts.ServiceLibrary.dll", "Vueling.XXX.Impl.ServiceLibrary.dll", "Vueling.XXX.Impl.ServiceLibrary.UnitTest.dll", "Vueling.XXX.Library.dll", "Vueling.XXX.Library.UnitTest.dll", "Vueling.XXX.DB.Infrastructure.dll", "Vueling.XXX.DB.Infrastructure.UnitTest.dll", "Vueling_XXX.Database.dll", "Vueling.XXX.WCF.WebService.IntegrationTest.dll", "Vueling.XXX.WCF.REST.WebService.IntegrationTest.dll", "Vueling.XXX.Subscriber.WindowsService.dll", "Vueling.XXX.Message.dll", "Vueling.XXX.Publisher.Contracts.ServiceLibrary.dll", "Vueling.XXX.Publisher.Impl.ServiceLibrary.dll", "Vueling.XXX.Publisher.WCF.WebService.dll", "Vueling.XXX.Model.dll"} }
            };

            var invoker = new WorkflowInvoker(target);

            _MockFactory = new MockFactory();
            _buildDetail = _MockFactory.CreateMock<IBuildDetail>();

            _buildDetail.IgnoreUnexpectedInvocations = true;
            _buildDetail.Expects.AtLeastOne.GetProperty<BuildStatus>(x => x.Status).WillReturn(BuildStatus.InProgress);
            _buildDetail.Expects.AtLeastOne.Method(x => x.Save());
            _buildDetail.Expects.AtLeastOne.GetProperty(x => x.DropLocation).WillReturn(@"D:\workspaces\tfs\Vueling.XXX\PRE\Vueling.XXX.ServiceLibrary\bin\Debug");

            invoker.Extensions.Add(_buildDetail.MockObject);

            try
            {
                var output = invoker.Invoke(parameters);
            }
            catch (Exception exception)
            {
                
                throw exception;
            }

            Assert.IsNotNull(target);
        }

        [TestMethod]
        public void CodeMetricsVuelingConfiguration()
        {
            var target = new TFSBuildExtensions.CodeMetrics.CodeMetricsActivity
            {
                BinariesDirectory = @"D:\workspaces\tfs\Vueling.Configuration\INT\Vueling.Configuration.Library\bin\Debug",
                OutputFile = @"D:\workspaces\tfs\Vueling.Configuration\INT\Vueling.Configuration.Library\bin\Debug\Vueling.Configuration.INT.64_20130430.1.log",
                ThresholdFile = @"D:\workspaces\tfs\BuildProcessTemplates\TFSBuildExtensions\SupportFiles\CodeMetricsThreshold.xml",
            };

            var parameters = new Dictionary<string, object> 
            { { "FilesToProcess", new string[] {"Vueling.Configuration.Library.dll"} } 
            };

            var invoker = new WorkflowInvoker(target);

            _MockFactory = new MockFactory();
            _buildDetail = _MockFactory.CreateMock<IBuildDetail>();

            _buildDetail.IgnoreUnexpectedInvocations = true;
            _buildDetail.Expects.AtLeastOne.GetProperty<BuildStatus>(x => x.Status).WillReturn(BuildStatus.InProgress);
            _buildDetail.Expects.AtLeastOne.Method(x => x.Save());
            _buildDetail.Expects.AtLeastOne.GetProperty(x => x.DropLocation).WillReturn(@"D:\workspaces\tfs\Vueling.Configuration\INT\Vueling.Configuration.Library\bin\Debug");

            invoker.Extensions.Add(_buildDetail.MockObject);

            try
            {
                var output = invoker.Invoke(parameters);
            }
            catch (Exception exception)
            {

                throw exception;
            }

            Assert.IsNotNull(target);
        }

    }
}