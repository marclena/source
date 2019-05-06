using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Vueling.DIRegister.WebUI.ServiceLibrary;

namespace Vueling.XXX.WebApi.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class ReflectionRegistrator : DIWebUI
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerBuilder"></param>
        protected override void CustomDependenciesRegister(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            RegisterApplicationServices(containerBuilder);
        }

        /// <summary>
        /// Set controller resolver
        /// </summary>
        /// <param name="container"></param>
        protected override void ResolveAfterBuildContainer(IContainer container)
        {
            var config = GlobalConfiguration.Configuration;
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
        }

        /// <summary>
        /// Register application services HERE
        /// </summary>
        /// <param name="containerBuilder"></param>
        private void RegisterApplicationServices(ContainerBuilder containerBuilder)
        {
            //REGISTER APPLICATION SERVICES
        }
    }
}