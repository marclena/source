using System;
using System.Collections.Generic;
using System.Linq;
using Vueling.Configuration.Library;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.DB.Infrastructure.Exceptions;
using System.Diagnostics;
using ATC.Taskling.Client.DIRegister.NetFramework;

namespace Vueling.XXX.DB.Infrastructure.Configuration
{
    [RegisterConfigurationAttribute]
    public class XXXInfrastructureConfiguration : IXXXInfrastructureConfiguration, IDatabaseConfiguration
    {
        protected const char CONFIGLISTSEPARATOR = ';';
        private readonly VuelingEnvironment _currentConfig;

        public XXXInfrastructureConfiguration()
        {
            try
            {
                _currentConfig = LoadCurrentConfig();
                LoadCustomSettings();
            }
            catch (ConfigurationInitializationException) { throw; }
            catch (Exception ex)
            {
                var errorMessage = string.Format("Error initializing configuration in {0}: {1}. {2}",
                    GetType().FullName, ex.Message, ex);

                Trace.TraceError(errorMessage);
                throw new ConfigurationInitializationException(errorMessage);
            }
        }

        private VuelingEnvironment LoadCurrentConfig()
        {
            if (!VuelingEnvironment.IsInitialized)
            {
                var errorMessage = string.Format("VuelingEnvironment not initialized in {0}.", GetType().FullName);

                Trace.TraceError(errorMessage);
                throw new ConfigurationInitializationException(errorMessage);
            }

            VuelingEnvironment.InitializeLibrary("Vueling.XXX.DB.Infrastructure");

            return VuelingEnvironment.Current;
        }

        private void LoadCustomSettings()
        {
            DatabaseTimeout = _currentConfig.GetCustomSetting<int>("Vueling.XXX.DB.Infrastructure.DatabaseTimeout");
            ConnectionString = _currentConfig.GetDataConnectionString("Vueling_XXX");
        }

        public int DatabaseTimeout { get; private set; }

        public string ConnectionString { get; private set; }

        private string FindKey(string keyVar)
        {
            return _currentConfig.GetCustomSetting(keyVar);
        }

        private List<string> FindKeyList(string keyVar)
        {
            var result = new List<string>();
            var findKey = this.FindKey(keyVar);
            if (!String.IsNullOrEmpty(findKey))
            {
                result = new List<string>(findKey.Split(CONFIGLISTSEPARATOR).Select(s => s.Trim()));
            }
            return result;
        }


    }
}