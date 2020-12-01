using Microsoft.TeamFoundation.Build.Client;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vueling.Build.Deploy.AWS.Contracts.ServiceLibrary;
using Vueling.Build.Deploy.AWS.Impl.ServiceLibrary;
using Vueling.Build.Deploy.AWS.Impl.ServiceLibrary.CodeDeploy;
using Vueling.Build.Deploy.AWS.Impl.ServiceLibrary.Configuration;
using Vueling.Build.Deploy.AWS.Impl.ServiceLibrary.EC2;
using Vueling.Build.Deploy.AWS.Impl.ServiceLibrary.ElasticBeanstalk;
using Vueling.Build.Deploy.AWS.Impl.ServiceLibrary.ELB;
using Vueling.Build.Deploy.AzurePlataform.Contracts.ServiceLibrary;
using Vueling.Build.Deploy.AzurePlataform.Impl.ServiceLibrary;
using Vueling.Build.Deploy.Config.DTO;
using Vueling.Build.Deploy.Proxy.Contracts.ServiceLibrary;
using Vueling.Build.Deploy.Proxy.Impl.ServiceLibrary;

namespace TFSBuildExtensions.Deployment
{
    [BuildActivity(HostEnvironmentOption.All)]
    public class DeployApplicationServiceActivity : BaseCodeActivity
    {
        [RequiredArgument]
        public InArgument<string> ApplicationName { get; set; }

        [RequiredArgument]
        public InArgument<string> Environment { get; set; }

        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }

        public InArgument<string> DestinationSubFolder { get; set; }

        [RequiredArgument]
        public InArgument<double> DeployTimeoutInSeconds { get; set; }

        [RequiredArgument]
        public InArgument<string> BucketKey { get; set; }

        [RequiredArgument]
        public InArgument<string> BucketName { get; set; }

        public InArgument<string> DeploymentConfigName { get; set; }

        public InArgument<string> DeploymentGroupName { get; set; }

        [RequiredArgument]
        public InArgument<bool> IsEnabledCodeDeployRegister { get; set; }

        [RequiredArgument]
        public InArgument<bool> IsEnabledCodeDeployDeploy { get; set; }

        [RequiredArgument]
        public InArgument<bool> IsEnabledS3Deploy { get; set; }

        [RequiredArgument]
        public InArgument<bool> IsELBGetInstancesFromAutoscalingNameEnabled { get; set; }

        [RequiredArgument]
        public InArgument<bool> IsEC2GetInstancesFromAutoscalingNameEnabled { get; set; }

        public InArgument<string> AutoscalingName { get; set; }

        public InArgument<string> ElasticLoadBalancerName { get; set; }

        [Description("")]
        public OutArgument<List<string>> ErrorMessageListArgument { get; set; }



        private IAzureDeployApplicationService azureDeployApplicationService;
        private IAmazonDeployApplicationService amazonDeployApplicationService;
        private IRDSService rDSService;
        private IElasticBeanstalkService elasticBeanstalkService;
        private IS3BucketService s3BucketService;
        private ICodeDeployService codeDeployService;
        private IEC2Service ec2Service;
        private IELBService elbService;

        private IBuildDeployAWSConfiguration buildDeployAWSConfiguration;
        private IInfrastructureAWSRDSConfiguration infrastructureAWSRDSConfiguration;
        private IInfrastructureAWSElasticBeansTalkConfiguration infrastructureAWSElasticBeansTalkConfiguration;
        private IInfrastructureAWSS3Configuration infrastructureAWSS3Configuration;
        private IInfrastructureAWSCodeDeployConfiguration infrastructureAWSCodeDeployConfiguration;
        private IInfrastructureEC2Configuration infrastructureEC2Configuration;
        private IInfrastructureELBConfiguration infrastructureELBConfiguration;

        private IEnvironmentService environmentService;
        private IApplicationVersionService applicationVersionService;
        private IResourceGroupService resourceGroupService;

        IDeployApplicationService deployApplicationService;
        private DeployRequest deployRequest;
        private DeployResponse deployResponse;

        protected override void InternalExecute()
        {
            InitializeServices();

            ConfigureRequest();

            deployResponse = deployApplicationService.Deploy(deployRequest);

            writeMessages(deployResponse);
        }

        private void writeMessages(DeployResponse deployResponse)
        {
            this.InformationMessageList = deployResponse.GetInformationMessages();
            this.WarningMessageList = deployResponse.GetWarningMessages();
            this.ErrorMessageList = deployResponse.GetErrorMessages();

            this.ErrorMessageListArgument.Set(this.ActivityContext, this.ErrorMessageList);
        }

        private void ConfigureRequest()
        {
            deployRequest = new DeployRequest
            {
                applicationName = this.ApplicationName.Get(this.ActivityContext),
                environment = this.Environment.Get(this.ActivityContext),
                filePath = this.FilePath.Get(this.ActivityContext),
                IsEnabledS3Deploy = this.IsEnabledS3Deploy.Get(this.ActivityContext),
                IsEnabledCodeDeployRegister = this.IsEnabledCodeDeployRegister.Get(this.ActivityContext),
                IsEnabledCodeDeployDeploy = this.IsEnabledCodeDeployDeploy.Get(this.ActivityContext),
                IsEC2GetInstancesFromAutoscalingNameEnabled = this.IsEC2GetInstancesFromAutoscalingNameEnabled.Get(this.ActivityContext),
                IsELBGetInstancesFromAutoscalingNameEnabled = this.IsELBGetInstancesFromAutoscalingNameEnabled.Get(this.ActivityContext),
            };

            deployRequest.codeDeployRequest = new CodeDeployRequest
            {
                ApplicationName = this.ApplicationName.Get(this.ActivityContext),
                BucketKey = this.BucketKey.Get(this.ActivityContext),
                BucketName = this.BucketName.Get(this.ActivityContext),
                DeploymentConfigName = this.DeploymentConfigName.Get(this.ActivityContext),
                DeploymentGroupName = this.DeploymentGroupName.Get(this.ActivityContext),
                DeployTimeout = TimeSpan.FromSeconds(this.DeployTimeoutInSeconds.Get(this.ActivityContext)),
                Description = this.ApplicationName.Get(this.ActivityContext) + " release " + Path.GetFileName(this.FilePath.Get(this.ActivityContext)),
                IgnoreApplicationStopFailures = false
            };


            deployRequest.s3Request = new S3Request
            {
                BucketName = this.BucketName.Get(this.ActivityContext),
                DestinationSubFolder = this.DestinationSubFolder.Get(this.ActivityContext)
            };

            deployRequest.ec2Request = new EC2Request
            {
                AutoscalingGroupName = this.AutoscalingName.Get(this.ActivityContext)
            };

            deployRequest.elbRequest = new ELBRequest
            {
                ELBName = this.ElasticLoadBalancerName.Get(this.ActivityContext)
            };
        }

        private void InitializeServices()
        {
            buildDeployAWSConfiguration = new BuildDeployAWSConfiguration();
            infrastructureAWSElasticBeansTalkConfiguration = new InfrastructureAWSElasticBeanstalkConfiguration();
            infrastructureAWSRDSConfiguration = new InfrastructureAWSRDSConfiguration();
            infrastructureAWSS3Configuration = new InfrastructureAWSS3Configuration();
            infrastructureAWSCodeDeployConfiguration = new InfrastructureAWSCodeDeployConfiguration();
            infrastructureELBConfiguration = new InfrastructureELBConfiguration();
            infrastructureEC2Configuration = new InfrastructureEC2Configuration();

            rDSService = new RDSService(buildDeployAWSConfiguration, infrastructureAWSRDSConfiguration);
            s3BucketService = new S3BucketService(buildDeployAWSConfiguration, infrastructureAWSS3Configuration);

            environmentService = new EnvironmentService(buildDeployAWSConfiguration, infrastructureAWSElasticBeansTalkConfiguration);
            applicationVersionService = new ApplicationVersionService(buildDeployAWSConfiguration);
            resourceGroupService = new ResourceGroupService();

            elasticBeanstalkService = new ElasticBeanstalkService(buildDeployAWSConfiguration, infrastructureAWSElasticBeansTalkConfiguration, s3BucketService,
                environmentService, applicationVersionService, resourceGroupService);

            codeDeployService = new CodeDeployService(buildDeployAWSConfiguration, infrastructureAWSCodeDeployConfiguration);

            ec2Service = new EC2Service(buildDeployAWSConfiguration, infrastructureEC2Configuration);
            elbService = new ELBService(buildDeployAWSConfiguration, infrastructureELBConfiguration, ec2Service);

            amazonDeployApplicationService = new AmazonDeployApplicationService(rDSService, elasticBeanstalkService, s3BucketService, codeDeployService, ec2Service, elbService);

            azureDeployApplicationService = new AzureDeployApplicationService();

            deployApplicationService = new DeployApplicationService(amazonDeployApplicationService, azureDeployApplicationService);
        }
    }
}