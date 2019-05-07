using Autofac;

namespace Vueling.XXX.WebApi.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class ReflectionRegistrator : WebApiRegistrator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerBuilder"></param>
        protected override void CustomDependenciesRegister(ContainerBuilder containerBuilder)
        {
            //containerBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            //RegisterApplicationServices(containerBuilder);
        }

    }
}