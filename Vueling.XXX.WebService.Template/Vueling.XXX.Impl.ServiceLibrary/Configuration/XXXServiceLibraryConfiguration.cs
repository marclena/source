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
            //return "1A;1B;1C;1D;1E;1F;2A;2B;2C;2D;2E;2F;3A;3B;3C;3D;3E;3F;4A;4B;4C;4D;4E;4F;5A;5B;5C;5D;5E;5F;6A;6B;6C;6D;6E;6F;7A;7B;7C;7D;7E;7F;8A;8B;8C;8D;8E;8F;9A;9B;9C;9D;9E;9F;10A;10B;10C;10D;10E;10F;11A;11B;11C;11D;11E;11F;12A;12B;12C;12D;12E;12F";
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
