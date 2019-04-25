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
using Vueling.XXX.WebApi.App_Start;

[assembly: OwinStartup(typeof(Vueling.XXX.WebApi.Startup))]

namespace Vueling.XXX.WebApi
{
    public class Startup
    {
        public const string ApplicationId = "Vueling.XXX.WebAPI";

        public void Configuration(IAppBuilder appBuilder)
        {
            var baseNamespace = typeof(Startup).Namespace;
            Trace.TraceInformation("Starting up....");

            VuelingEnvironment.InitializeCurrentForApplication(baseNamespace);
            Trace.TraceInformation("Vueling Configuration initialized");
            
            GlobalConfiguration.Configure(WebApiConfig.Register);

            
            //if (bool.Parse(VuelingEnvironment.Current.GetCustomSetting(baseNamespace + ".EnableSwagger")))
            //{
            //    Trace.TraceInformation("Configuring Swagger");
            //    GlobalConfiguration.Configure(SwaggerConfig.Register);
            //    Trace.TraceInformation("Swagger configured");
            //}

            Trace.TraceInformation("Configuring Swagger");
            GlobalConfiguration.Configure(SwaggerConfig.Register);
            Trace.TraceInformation("Swagger configured");


            RegisterDependencies();
            Trace.TraceInformation("Dependecies registered");

            Trace.TraceInformation("Configuring Authentication Server");
            ConfigureAuth(appBuilder);
            Trace.TraceInformation("Authentication Server configured");

            Trace.TraceInformation("Using Web API....");
            appBuilder.UseWebApi(GlobalConfiguration.Configuration);
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