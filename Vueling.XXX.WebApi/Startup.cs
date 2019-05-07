using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http;
using Vueling.Configuration.Library;
using Vueling.DIRegister.AssemblyDiscovery.ServiceLibrary;
using System.Reflection;
using Vueling.DIRegister.AssemblyDiscovery.ServiceLibrary.DTO;
using Vueling.XXX.WebApi.Helpers;
using ATC.Swagger.Standard.Extension;
using Microsoft.Owin.Cors;

[assembly: OwinStartup(typeof(Vueling.XXX.WebApi.Startup))]

namespace Vueling.XXX.WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Application Id
        /// </summary>
        public const string ApplicationId = "Vueling.XXX.WebAPI";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="appBuilder"></param>
        public void Configuration(IAppBuilder appBuilder)
        {
            var baseNamespace = typeof(Startup).Namespace;
            Trace.TraceInformation("Starting up....");

            VuelingEnvironment.InitializeCurrentForApplication(baseNamespace);
            Trace.TraceInformation("Vueling Configuration initialized");
            
            GlobalConfiguration.Configure(WebApiConfig.Register);

            Trace.TraceInformation("Configuring Swagger");
            GlobalConfiguration.Configuration.AddCustomSwaggerGen(ApplicationId);
            Trace.TraceInformation("Swagger configured");

            RegisterDependencies();
            Trace.TraceInformation("Dependecies registered");

            Trace.TraceInformation("Configuring Authentication Server");
            ConfigureAuth(appBuilder);
            Trace.TraceInformation("Authentication Server configured");

            Trace.TraceInformation("Using Web API....");
            appBuilder.UseWebApi(GlobalConfiguration.Configuration);
            appBuilder.UseCors(CorsOptions.AllowAll);
            
            GlobalConfiguration.Configuration.EnsureInitialized();

        }

        private void ConfigureAuth(IAppBuilder appBuilder)
        {
            //Configure authorization HERE
        }

        private void RegisterDependencies()
        {
            var registerDefinition = BuildRegisterDefinition();
            var registrator = new ReflectionRegistrator();
            registrator.EnableVerboseTrace();
            registrator.RegisterDependencies(registerDefinition);
        }

        private static RegisterDefinition BuildRegisterDefinition()
        {
            return new RegisterDefinition
            {
                ExecutingAssembly = Assembly.GetExecutingAssembly(),
                ConfigurationLifetimeScope = LifetimeScopes.InstancePerLifetimeScope,
                DbContextLifetimeScope = LifetimeScopes.InstancePerLifetimeScope,
                DefaultServiceLifetimeScope = LifetimeScopes.InstancePerDependency,
                InstanciateSingleInstanceServicesAfterRegister = true,
                InstanciateAllServicesAfterRegister = false,
                IgnoreTypes = new List<Type>(),
                AdditionalEntryServices = new List<Type>()
            };
        }
    }
}