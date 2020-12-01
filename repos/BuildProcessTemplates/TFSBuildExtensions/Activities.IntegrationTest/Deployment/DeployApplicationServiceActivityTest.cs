using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Activities;
using System.IO;

namespace Activities.IntegrationTest.Deployment
{
    [TestClass]
    public class DeployApplicationServiceActivityTest
    {
        [TestMethod]
        public void DeploySkysales920Request()
        {
            var target = new TFSBuildExtensions.Deployment.DeployApplicationServiceActivity
            {
                ApplicationName = "SkySales",
                BucketKey = "SKYSALES.PRO.920.Branch_20170405.2.zip",
                BucketName = "skysales-revisions",
                DeploymentConfigName = "CodeDeployDefault.OneAtATime",
                DeploymentGroupName = "Vueling-SkySales-App",
                DeployTimeoutInSeconds = 5400,
                Environment = "PRO",
                FilePath = new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\fake\\SKYSALES.PRO.920.Branch_20170405.2.zip",
                IsELBGetInstancesFromAutoscalingNameEnabled = false,
                IsEC2GetInstancesFromAutoscalingNameEnabled = false,
                IsEnabledCodeDeployDeploy = false,
                IsEnabledCodeDeployRegister = true,
                IsEnabledS3Deploy = true,

            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Assert.IsNotNull(output);
        }

        [TestMethod]
        public void DeploySkySalesToAWSPoolsRequest()
        {
            var target = new TFSBuildExtensions.Deployment.DeployApplicationServiceActivity
            {
                ApplicationName = "SkySales",
                BucketKey = "SKYSALES.PRO.920.Branch_20170405.2.zip",
                BucketName = "skysales-revisions",
                ElasticLoadBalancerName = "skysales-elb",
                DeploymentConfigName = "CodeDeployDefault.OneAtATime",
                DeploymentGroupName = "Vueling-SkySales-App",
                DeployTimeoutInSeconds = 5400,
                Environment = "PRO",
                FilePath = "\\\\wbcnvuebld\\Drops\\SKYSALES.PRO.920.Branch\\SKYSALES.PRO.920.Branch_20170405.2\\zips\\SKYSALES.PRO.920.Branch_20170405.2.zip",
                IsELBGetInstancesFromAutoscalingNameEnabled = true,
                IsEnabledCodeDeployDeploy = true,
                IsEnabledCodeDeployRegister = false,
                IsEnabledS3Deploy = false,
                IsEC2GetInstancesFromAutoscalingNameEnabled = false
            };

            var invoker = new WorkflowInvoker(target);

            var output = invoker.Invoke();

            Assert.IsNotNull(output); 
        }
    }
}
