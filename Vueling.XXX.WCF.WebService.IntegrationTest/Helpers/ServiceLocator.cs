﻿using Autofac;
using System;
using System.Collections.Generic;
using System.Reflection;
using Vueling.Configuration.Library;
using Vueling.DIRegister.AssemblyDiscovery.ServiceLibrary;
using Vueling.DIRegister.AssemblyDiscovery.ServiceLibrary.DTO;

namespace Vueling.XXX.WCF.WebService.IntegrationTest.Helpers
{
    internal class ServiceLocator
    {
        const string applicationId = "Vueling.XXX.WCF.WebService.IntegrationTest";
        static IContainer container;

        static ServiceLocator()
        {
            if (!VuelingEnvironment.IsInitialized)
            {
                VuelingEnvironment.InitializeCurrentForApplication(applicationId);
            }

            var registerDefinition = new RegisterDefinition
            {
                //ExecutingAssembly = Assembly.Load("Vueling.XXX.WCF.WebService"),
                ExecutingAssembly = Assembly.Load(applicationId),
                ConfigurationLifetimeScope = LifetimeScopes.InstancePerLifetimeScope,
                DbContextLifetimeScope = LifetimeScopes.InstancePerLifetimeScope,
                DefaultServiceLifetimeScope = LifetimeScopes.InstancePerDependency,
                InstanciateSingleInstanceServicesAfterRegister = true,
#if (DEBUG)
                InstanciateAllServicesAfterRegister = true,
#endif
                AdditionalEntryServices = new List<Type>
                {
                    typeof(Vueling.XXX.Contracts.ServiceLibrary.IAircraftMaintenanceApplicationService),
                    typeof(SeatReservationForAircraftsWebService)
                    
                }
            };

            var registrator = new ReflectionRegistrator();
            //registrator.EnableVerboseTrace();
            registrator.RegisterDependencies(registerDefinition);

            container = registrator.Container;
        }

        internal static T Resolve<T>()
        {
            return DIRegister.Custom.ServiceLibrary.DICustom.Retrieve<T>();
        }
    }
}
