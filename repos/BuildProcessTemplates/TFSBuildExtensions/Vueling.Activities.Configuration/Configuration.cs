using System.Collections.Generic;
using Vueling.ToolChain.Configuration.Contracts.ServiceLibrary;
using Vueling.ToolChain.Configuration.Impl.ServiceLibrary;

namespace Vueling.Activities.Configuration
{

    public static class Configuration
    {
        private static IConfiguration _currentconfig;

        private static string _tfsurl;
        private static string _domain;
        private static string _teamProject;
        private static string _winScpPath;
        private static string _tfsuser;
        private static string _tfspassword;
        private static string _folderBase;
        private static string _tfsdatatier;
        private static string _sqldatatier;
        private static string _tfsuserdb;
        private static string _tfspassworddb;
        private static string _dataWarehouseConnection;
        private static string _dataToolsConnection;
        private static string _tfsoperationsuser;
        private static string _tfsoperationspassword;
        private static string _dataTfsOperationsConnection;
        private static string _navitaireWebDeployManagerServer;
        private static int _navitaireWebDeployManagerServerPort;
        private static string _nugetLocalRepositoryUrl;
        private static string _nugetServerRepositoryUrl;
        private static string _nugetServerTestRepositoryUrl;
        private static string _rabbitMQAdministratorsUser;
        private static string _rabbitMQAdministratorsPassword;
        private static List<string> targetEnvironmentNavitaire = null;
        private static string releaseFile;
        private static string binariesDropLocation;
        private static string partialPathFilteredAssemblies;
        private static string partialPathLibAssemblyExceptions;
        private static string nugetPackagesProjectTypes;
        private static List<string> validateFileEncodingExcludedExtensions;
        private static List<string> skySalesDeploymentEmailNotification;

        static Configuration()
        {
            _currentconfig = ConfigurationProvider.Instance.GetConfiguration("Vueling.Activities.Configuration");

            LoadConfigValues();
        }

        private static void LoadConfigValues()
        {
            _tfsurl = _currentconfig.GetSetting("tfsurl");
            _domain = _currentconfig.GetSetting("domain");
            _teamProject = _currentconfig.GetSetting("teamProject");
            _winScpPath = _currentconfig.GetSetting("winScpPath");
            _tfsuser = _currentconfig.GetSetting("tfsuser");
            _tfspassword = _currentconfig.GetSetting("tfspassword");
            _folderBase = _currentconfig.GetSetting("folderBase");
            _tfsdatatier = _currentconfig.GetSetting("tfsdatatier");
            _sqldatatier = _currentconfig.GetSetting("sqldatatier");
            _tfsuserdb = _currentconfig.GetSetting("tfsuserdb");
            _tfspassworddb = _currentconfig.GetSetting("tfspassworddb");
            _tfsoperationsuser = _currentconfig.GetSetting("tfsoperationsuser");
            _tfsoperationspassword = _currentconfig.GetSetting("tfsoperationspassword");
            _navitaireWebDeployManagerServer = _currentconfig.GetSetting("NavitaireWebDeployManagerServer");
            _navitaireWebDeployManagerServerPort = int.Parse(_currentconfig.GetSetting("NavitaireWebDeployManagerServerPort"));
            _nugetLocalRepositoryUrl = _currentconfig.GetSetting("NugetLocalRepositoryUrl");
            _nugetServerRepositoryUrl = _currentconfig.GetSetting("NugetServerRepositoryUrl");
            _nugetServerTestRepositoryUrl = _currentconfig.GetSetting("NugetServerTestRepositoryUrl");
            _rabbitMQAdministratorsUser = _currentconfig.GetSetting("RabbitMQAdministratorsUser");
            _rabbitMQAdministratorsPassword = _currentconfig.GetSetting("RabbitMQAdministratorsPassword");

            string temptargetEnvironmentNavitaire = _currentconfig.GetSetting("targetEnvironmentNavitaire");
            targetEnvironmentNavitaire = new List<string>();
            foreach (var s in temptargetEnvironmentNavitaire.Split(','))
            {
                targetEnvironmentNavitaire.Add(s);
            }

            string temptargetEnvironmentTLVBCN = _currentconfig.GetSetting("targetEnvironmentTLVBCN");
            targetEnvironmentTLVBCN = new List<List<string>>();
            int counter = 0;
            //- pool separator
            foreach (var pool in temptargetEnvironmentTLVBCN.Split('-'))
            {
                targetEnvironmentTLVBCN.Add(new List<string>());

                //, server separator
                foreach (var s in pool.ToString().Split(','))
                {
                    targetEnvironmentTLVBCN[counter].Add(s);
                }
                counter++;
            }

            _dataWarehouseConnection = "Data source=" + tfsdatatier + ";Initial Catalog=Tfs_Warehouse;User ID=" + tfsuserdb + ";Password=" + tfspassworddb;
            _dataToolsConnection = "Data source=" + tfsdatatier + ";Initial Catalog=Vueling_TFS_Tools;User ID=" + tfsuserdb + ";Password=" + tfspassworddb;
            _dataTfsOperationsConnection = "Data source=" + sqldatatier + ";Initial Catalog=Vueling_TfsOperations;User ID=" + tfsoperationsuser + ";Password=" + tfsoperationspassword;

            releaseFile = _currentconfig.GetSetting("ReleaseFile");

            binariesDropLocation = _currentconfig.GetSetting("BinariesDropLocation");

            partialPathLibAssemblyExceptions = _currentconfig.GetSetting("PartialPathLibAssemblyExceptions");

            partialPathFilteredAssemblies = _currentconfig.GetSetting("PartialPathFilteredAssemblies");

            nugetPackagesProjectTypes = _currentconfig.GetSetting("NugetPackagesProjectTypes");

            validateFileEncodingExcludedExtensions = new List<string>();

            foreach (var excludedFileExtension in _currentconfig.GetSetting("ValidateFileEncodingExcludedExtensions").Split(','))
            {
                validateFileEncodingExcludedExtensions.Add(excludedFileExtension);
            }

            skySalesDeploymentEmailNotification = new List<string>();

            foreach (var email in _currentconfig.GetSetting("SkySalesDeploymentEmailNotification").Split(','))
            {
                skySalesDeploymentEmailNotification.Add(email);
            }
        }

        public static string tfsurl
        {
            get { return Configuration._tfsurl; }
            set { Configuration._tfsurl = value; }
        }

        public static string domain
        {
            get { return Configuration._domain; }
            set { Configuration._domain = value; }
        }

        public static string teamProject
        {
            get { return Configuration._teamProject; }
            set { Configuration._teamProject = value; }
        }

        public static string winScpPath
        {
            get { return Configuration._winScpPath; }
            set { Configuration._winScpPath = value; }
        }

        public static string tfsuser
        {
            get { return Configuration._tfsuser; }
            set { Configuration._tfsuser = value; }
        }

        public static string tfspassword
        {
            get { return Configuration._tfspassword; }
            set { Configuration._tfspassword = value; }
        }

        public static string folderBase
        {
            get { return Configuration._folderBase; }
            set { Configuration._folderBase = value; }
        }

        public static string tfsdatatier
        {
            get { return Configuration._tfsdatatier; }
            set { Configuration._tfsdatatier = value; }
        }

        public static string sqldatatier
        {
            get { return Configuration._sqldatatier; }
            set { Configuration._sqldatatier = value; }
        }

        public static string tfsuserdb
        {
            get { return Configuration._tfsuserdb; }
            set { Configuration._tfsuserdb = value; }
        }

        public static string tfspassworddb
        {
            get { return Configuration._tfspassworddb; }
            set { Configuration._tfspassworddb = value; }
        }

        public static string DataWarehouseConnection
        {
            get { return Configuration._dataWarehouseConnection; }
            set { Configuration._dataWarehouseConnection = value; }
        }

        public static string DataToolsConnection
        {
            get { return Configuration._dataToolsConnection; }
            set { Configuration._dataToolsConnection = value; }
        }

        public static string tfsoperationsuser
        {
            get { return Configuration._tfsoperationsuser; }
            set { Configuration._tfsoperationsuser = value; }
        }

        public static string tfsoperationspassword
        {
            get { return Configuration._tfsoperationspassword; }
            set { Configuration._tfsoperationspassword = value; }
        }

        public static string DataTfsOperationsConnection
        {
            get { return Configuration._dataTfsOperationsConnection; }
            set { Configuration._dataTfsOperationsConnection = value; }
        }

        public static string NavitaireWebDeployManagerServer
        {
            get { return Configuration._navitaireWebDeployManagerServer; }
            set { Configuration._navitaireWebDeployManagerServer = value; }
        }

        public static int NavitaireWebDeployManagerServerPort
        {
            get { return Configuration._navitaireWebDeployManagerServerPort; }
            set { Configuration._navitaireWebDeployManagerServerPort = value; }
        }

        public static string NugetLocalRepositoryUrl
        {
            get { return Configuration._nugetLocalRepositoryUrl; }
            set { Configuration._nugetLocalRepositoryUrl = value; }
        }

        public static string NugetServerRepositoryUrl
        {
            get { return Configuration._nugetServerRepositoryUrl; }
            set { Configuration._nugetServerRepositoryUrl = value; }
        }

        public static string NugetServerTestRepositoryUrl
        {
            get { return Configuration._nugetServerTestRepositoryUrl; }
            set { Configuration._nugetServerTestRepositoryUrl = value; }
        }

        public static string RabbitMQAdministratorsUser
        {
            get { return Configuration._rabbitMQAdministratorsUser; }
            set { Configuration._rabbitMQAdministratorsUser = value; }
        }

        public static string RabbitMQAdministratorsPassword
        {
            get { return Configuration._rabbitMQAdministratorsPassword; }
            set { Configuration._rabbitMQAdministratorsPassword = value; }
        }

        public static List<string> TargetEnvironmentNavitaire
        {
            get { return Configuration.targetEnvironmentNavitaire; }
        }

        private static List<List<string>> targetEnvironmentTLVBCN = null;

        public static List<List<string>> TargetEnvironmentTLVBCN
        {
            get { return targetEnvironmentTLVBCN; }
        }

        public static string ReleaseFile
        {
            get { return Configuration.releaseFile; }
            set { Configuration.releaseFile = value; }
        }

        public static string BinariesDropLocation
        {
            get { return Configuration.binariesDropLocation; }
            set { Configuration.binariesDropLocation = value; }
        }

        public static string PartialPathFilteredAssemblies
        {
            get { return Configuration.partialPathFilteredAssemblies; }
            set { Configuration.partialPathFilteredAssemblies = value; }
        }

        public static string PartialPathLibAssemblyExceptions
        {
            get { return Configuration.partialPathLibAssemblyExceptions; }
            set { Configuration.partialPathLibAssemblyExceptions = value; }
        }

        public static string NugetPackagesProjectTypes
        {
            get { return Configuration.nugetPackagesProjectTypes; }
            set { Configuration.nugetPackagesProjectTypes = value; }
        }

        public static List<string> ValidateFileEncodingExcludedExtensions
        {
            get { return Configuration.validateFileEncodingExcludedExtensions; }
            set { Configuration.validateFileEncodingExcludedExtensions = value; }
        }
        public static List<string> SkySalesDeploymentEmailNotification
        {
            get { return skySalesDeploymentEmailNotification; }
            set { skySalesDeploymentEmailNotification = value; }
        }
    }
}