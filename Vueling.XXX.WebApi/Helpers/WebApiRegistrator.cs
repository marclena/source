using Autofac;
using Autofac.Integration.WebApi;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using Vueling.DIRegister.AssemblyDiscovery.ServiceLibrary;
using Vueling.DIRegister.AssemblyDiscovery.ServiceLibrary.Services;
using Vueling.DIRegister.Autofac.ServiceLibrary;

namespace Vueling.XXX.WebAPI.Helpers
{
    /// <summary>
    /// 
    /// </summary>
    [SuppressMessage("CustomRules.Maintenability", "VY1000:GlobalNotUseServiceLocatorPattern")]
    public class WebApiRegistrator : ReflectionRegister
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="registerDefinition"></param>
        [SuppressMessage("CustomRules.Maintenability", "VY1000:GlobalNotUseServiceLocatorPattern")]
        public void RegisterDependencies(RegisterDefinition registerDefinition)
        {
            try
            {
                ValidateRegisterDefinition(registerDefinition);

                var containerBuilder = RegisterDependenciesFromWebApi(registerDefinition);

                var container = BuildContainer(containerBuilder);

                InstanciateServices(registerDefinition, container, false);

                ResolveAfterBuildContainer(container);
            }
            catch (System.Exception ex)
            {
                Trace.TraceError("Registration process aborted due to following exception: {0}", ex.GetDetails());
                throw;
            }
            finally
            {
                TryPrintTraces();

                TracerSingleton.Instance.CleanUp();

                DataStoreSingleton.Instance.CleanUp();
            }
        }

        [SuppressMessage("CustomRules.Maintenability", "VY1000:GlobalNotUseServiceLocatorPattern")]
        private ContainerBuilder RegisterDependenciesFromWebApi(RegisterDefinition registerDefinition)
        {
            var containerBuilder = new ContainerBuilder();

            DataStoreSingleton.Instance.RegisterDefinition = registerDefinition;

            DataStoreSingleton.Instance.Initialize();

            containerBuilder.RegisterApiControllers(registerDefinition.ExecutingAssembly);

            BuildRegistrationInfo<ApiController>(containerBuilder, false);

            BuildRegistrationInfoFromAdditionalEntries();

            RegisterTypesInContainer(containerBuilder);

            CustomDependenciesRegister(containerBuilder);
            
            return containerBuilder;
        }

        [SuppressMessage("CustomRules.Maintenability", "VY1000:GlobalNotUseServiceLocatorPattern")]
        private static IContainer BuildContainer(ContainerBuilder containerBuilder)
        {
            var container = containerBuilder.Build();
            var config = GlobalConfiguration.Configuration;
            config.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            return container;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerBuilder"></param>
        [SuppressMessage("CustomRules.Maintenability", "VY1000:GlobalNotUseServiceLocatorPattern")]
        protected virtual void CustomDependenciesRegister(ContainerBuilder containerBuilder)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        [SuppressMessage("CustomRules.Maintenability", "VY1000:GlobalNotUseServiceLocatorPattern")]
        protected virtual void ResolveAfterBuildContainer(IContainer container)
        {
        }
    }
}