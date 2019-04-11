using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Vueling.Configuration.Library;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.Library.Exceptions;

namespace Vueling.XXX.Library.Configuration
{
    [RegisterConfigurationAttribute]
    public class XXXLibraryConfiguration : IXXXLibraryConfiguration
    {
        protected const char CONFIGLISTSEPARATOR = ';';
        private readonly VuelingEnvironment _currentConfig;

        public int TimeSalesCloseBeforeFlight { get; private set; }
        public int MaxJourneysAllowedByBooking { get; private set; }
        public string PartialCodeForAgencyAgent { get; private set; }
        public string PartialCodeForCorporateAgent { get; private set; }

        public XXXLibraryConfiguration()
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

            VuelingEnvironment.InitializeLibrary("Vueling.XXX.Library");

            return VuelingEnvironment.Current;
        }

        private void LoadCustomSettings()
        {
            TimeSalesCloseBeforeFlight = Convert.ToInt32(FindKey("Vueling.XXX.Library.timeSalesCloseBeforeFlight"), CultureInfo.InvariantCulture);

            //TODO: set in appconfig
            MaxJourneysAllowedByBooking = 2;
            PartialCodeForAgencyAgent = "agency";
            PartialCodeForCorporateAgent = "corp";
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

    }
}
