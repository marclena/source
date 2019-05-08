using Autofac;
using Vueling.Maestros.Settings.Generic.Contracts.ServiceLibrary;
using Vueling.Maestros.Settings.Generic.DB.Infrastructure.Configuration;
using Vueling.Maestros.Settings.Generic.DB.Infrastructure.Repositories;
using Vueling.Maestros.Settings.Generic.Impl.ServiceLibrary;
using Vueling.Maestros.Settings.Generic.Impl.ServiceLibrary.InfrastructureContracts;

namespace Vueling.XXX.WebAPI.Helpers
{
    /// <summary>
    /// Services registration
    /// </summary>
    public class ReflectionRegistrator : WebApiRegistrator
    {
        /// <summary>
        /// Register your custom services
        /// </summary>
        /// <param name="containerBuilder">Autofac container builder</param>
        protected override void CustomDependenciesRegister(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterType<GenericSettingsApplicationService>().As<IGenericSettingsServiceContract>();
            containerBuilder.RegisterType<GenericSettingsRepository>().As<IGenericSettingsRepository>();
            containerBuilder.RegisterType<SettingsInfrastructureConfiguration>().As<ISettingsInfrastructureConfiguration>();

        }

    }
}