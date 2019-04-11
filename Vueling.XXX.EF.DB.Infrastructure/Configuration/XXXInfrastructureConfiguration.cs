using System;
using Vueling.Configuration.Library;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.EF.DB.Infrastructure.Exceptions;
using System.Diagnostics;

namespace Vueling.XXX.EF.DB.Infrastructure.Configuration
{
    [RegisterConfiguration]
    public class XXXInfrastructureConfiguration : IXXXInfrastructureConfiguration
    {
        private readonly VuelingEnvironment _currentConfig;
        public string DatabaseConnectionString { get; private set; }

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

            return VuelingEnvironment.Current;
        }

        private void LoadCustomSettings()
        {
            //TODO:
            //DatabaseConnectionString = "Data Source=(local);Initial Catalog=Vueling_XXX;Integrated Security=SSPI;MultipleActiveResultSets=True;";
            //DatabaseConnectionString = "Data Source=(local);Initial Catalog=Vueling_XXX;User ID=xxx.inf;Password=xxx.inf;";
            DatabaseConnectionString = VuelingEnvironment.Current.GetDataConnectionString("Vueling_XXX");
        }

    }
}