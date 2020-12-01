using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Vueling.Configuration.Library;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.Impl.ServiceLibrary.Exceptions;

namespace Vueling.XXX.WebUI.Configuration
{
    [RegisterConfiguration]
    [RegisterOnActivated]
    public class XXXWebUIConfiguration : IXXXWebUIConfiguration
    {

        protected const char CONFIGLISTSEPARATOR = ';';
        private readonly VuelingEnvironment _currentConfig;

        public int DefaultGridPageSize { get; private set; }

        public XXXWebUIConfiguration()
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
                    this.GetType().FullName, ex.Message, ex);

                Trace.TraceError(errorMessage);
                throw new ConfigurationInitializationException(errorMessage);
            }
        }

        private VuelingEnvironment LoadCurrentConfig()
        {
            if (!VuelingEnvironment.IsInitialized)
            {
                var errorMessage = string.Format("VuelingEnvironment not initialized in {0}.",
                    this.GetType().FullName);

                Trace.TraceError(errorMessage);
                throw new ConfigurationInitializationException(errorMessage);
            }

            return VuelingEnvironment.Current;
        }

        private void LoadCustomSettings()
        {
            //TODO: set in app.config
            DefaultGridPageSize = 10;
        }

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

        [RegisterActionOnActivated]
        public void PrintOnActivatedMethod()
        {
            Trace.TraceInformation("Invoked PrintOnActivatedMethod");
        }

    }
}
