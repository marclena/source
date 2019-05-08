using Autofac;

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
            //Register your services here
        }

    }
}