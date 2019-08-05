using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Vueling.Configuration.Library;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.Impl.ServiceLibrary.Exceptions;
using Vueling.XXX.Library.Entities;

namespace Vueling.XXX.Impl.ServiceLibrary.Configuration
{
    [RegisterConfigurationAttribute]
    public class XXXServiceLibraryConfiguration : IXXXServiceLibraryConfiguration
    {

        protected const char CONFIGLISTSEPARATOR = ';';
        private readonly VuelingEnvironment _currentConfig;

        public XXXServiceLibraryConfiguration()
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

            VuelingEnvironment.InitializeLibrary("Vueling.XXX.Impl.ServiceLibrary");

            return VuelingEnvironment.Current;
        }

        private void LoadCustomSettings()
        {
            List<string> flightSeatsConfigurationAsStringList = FindKeyList("Vueling.XXX.Impl.ServiceLibrary.FlightSeatsConfiguration");

            FlightSeatsConfiguration = new List<Seat>();

            foreach (var seatAsString in flightSeatsConfigurationAsStringList)
            {
                var newSeat = new Seat
                {
                    Availability = AvailabilityEnum.Available,
                    Column = seatAsString.Substring(seatAsString.Length - 1),
                    Row = seatAsString.Substring(0, seatAsString.Length - 1)
                };

                FlightSeatsConfiguration.Add(newSeat);
            }
        }

        public List<Seat> FlightSeatsConfiguration { get; private set; }

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
