using System.Collections.Generic;

namespace TFSBuildExtensions.ConfigurationSetupEnvironment
{
    public static class ConfigurationWorkflow
    {
        public static string DataWareHouseConnection{ 
            get 
            { 
                return Vueling.Activities.Configuration.Configuration.DataWarehouseConnection; 
            } 
        }

        public static string DataToolsConnection {
            get
            {
                return Vueling.Activities.Configuration.Configuration.DataToolsConnection;
            }
        }

        public static string DataTfsOperationsConnection {
            get
            {
                return Vueling.Activities.Configuration.Configuration.DataTfsOperationsConnection;
            }
        }

        public static string folderBase {
            get
            {
                return Vueling.Activities.Configuration.Configuration.folderBase;
            }
        }

        public static string NugetLocalRepositoryUrl
        {
            get
            {
                return Vueling.Activities.Configuration.Configuration.NugetLocalRepositoryUrl;
            }
        }

        public static string NugetServerRepositoryUrl
        {
            get
            {
                return Vueling.Activities.Configuration.Configuration.NugetServerRepositoryUrl;
            }
        }

        public static string NugetServerTestRepositoryUrl
        {
            get
            {
                return Vueling.Activities.Configuration.Configuration.NugetServerTestRepositoryUrl;
            }
        }

        public static string ReleaseFile
        {
            get
            {
                return Vueling.Activities.Configuration.Configuration.ReleaseFile;
            }
        }

        public static string BinariesDropLocation
        {
            get
            {
                return Vueling.Activities.Configuration.Configuration.BinariesDropLocation;
            }
        }

        public static List<string> TargetEnvironmentNavitaire
        {
            get
            {
                return Vueling.Activities.Configuration.Configuration.TargetEnvironmentNavitaire;
            }
        }

        public static List<List<string>> TargetEnvironmentTLVBCN
        {
            get
            {
                return Vueling.Activities.Configuration.Configuration.TargetEnvironmentTLVBCN;
            }
        }

        public static string PartialPathFilteredAssemblies
        {
            get { return Vueling.Activities.Configuration.Configuration.PartialPathFilteredAssemblies; }
            set { Vueling.Activities.Configuration.Configuration.PartialPathFilteredAssemblies = value; }
        }

        public static string PartialPathLibAssemblyExceptions
        {
            get { return Vueling.Activities.Configuration.Configuration.PartialPathLibAssemblyExceptions; }
            set { Vueling.Activities.Configuration.Configuration.PartialPathLibAssemblyExceptions = value; }
        }

        public static List<string> ValidateFileEncodingExcludedExtensions
        {
            get
            {
                return Vueling.Activities.Configuration.Configuration.ValidateFileEncodingExcludedExtensions;
            }
        }

        public static List<string> SkySalesDeploymentEmailNotification
        {
            get
            {
                return Vueling.Activities.Configuration.Configuration.SkySalesDeploymentEmailNotification;
            }
        }
    }
}