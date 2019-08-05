﻿using System;
using Vueling.Configuration.Library;
using Vueling.Extensions.Library.DI;
using Vueling.XXX.EF.DB.Infrastructure.Exceptions;
using System.Diagnostics;
using ATC.Taskling.Client.DIRegister.NetFramework;
namespace Vueling.XXX.EF.DB.Infrastructure.Configuration
{
    [RegisterConfiguration]
    public class InfrastructureConfiguration : IInfrastructureConfiguration, IDatabaseConfiguration
    {
        private readonly VuelingEnvironment _currentConfig;
        public string DatabaseConnectionString { get; private set; }

        public InfrastructureConfiguration()
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
            DatabaseConnectionString = VuelingEnvironment.Current.GetDataConnectionString("Vueling_XXX");
        }
        public int DatabaseTimeout { get; private set; }

        public string ConnectionString { get; private set; }
    }
}