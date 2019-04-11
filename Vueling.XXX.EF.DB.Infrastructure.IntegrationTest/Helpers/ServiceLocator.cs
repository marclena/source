using Autofac;
using System;
using System.Collections.Generic;
using System.Reflection;
using Vueling.Configuration.Library;
using Vueling.DIRegister.AssemblyDiscovery.ServiceLibrary;
using Vueling.DIRegister.AssemblyDiscovery.ServiceLibrary.DTO;

namespace Vueling.XXX.EF.DB.Infrastructure.IntegrationTest
{
    internal class ServiceLocator
    {
        const string applicationId = "Vueling.XXX.EF.DB.Infrastructure.IntegrationTest";
        static IContainer container;

        static ServiceLocator()
        {
            if (!VuelingEnvironment.IsInitialized)
            {
                VuelingEnvironment.InitializeCurrentForApplication(applicationId);
            }

            var registerDefinition = new RegisterDefinition
            {
                ExecutingAssembly = Assembly.Load("Vueling.XXX.Library"),
                ConfigurationLifetimeScope = LifetimeScopes.InstancePerLifetimeScope,
                DbContextLifetimeScope = LifetimeScopes.InstancePerLifetimeScope,
                DefaultServiceLifetimeScope = LifetimeScopes.InstancePerDependency,
                InstanciateSingleInstanceServicesAfterRegister = true,
#if (DEBUG)
                InstanciateAllServicesAfterRegister = true,
#endif
                AdditionalEntryServices = new List<Type>
                {
                    typeof(Vueling.XXX.Library.InfrastructureContracts.IUnitOfWorkBooking),
                }
            };

            var registrator = new Vueling.XXX.EF.DB.Infrastructure.IntegrationTest.Helpers.ReflectionRegistrator();
            registrator.RegisterDependencies(registerDefinition);

            container = registrator.Container;
        }

        internal static T Resolve<T>()
        {
            return DIRegister.Custom.ServiceLibrary.DICustom.Retrieve<T>();
        }
    }
}
