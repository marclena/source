using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using System.ServiceModel.Activation;
using Vueling.Configuration.Library;
using Autofac.Integration.Wcf;
using Vueling.DIRegister.AssemblyDiscovery.ServiceLibrary;
using Vueling.XXX.WCF.REST.WebService.Helpers;
using System.Diagnostics;
using System.Reflection;
using Vueling.DIRegister.AssemblyDiscovery.ServiceLibrary.DTO;

namespace Vueling.XXX.WCF.REST.WebService
{
    public class Global : HttpApplication
    {
        const string ApplicationId = "Vueling.XXX.WCF.REST.WebService";

        void Application_Start(object sender, EventArgs e)
        {
            try
            {
                VuelingEnvironment.InitializeCurrentForApplication(ApplicationId);

                RegisterDependencies();

                RegisterRoutes();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                HttpRuntime.UnloadAppDomain(); // Make sure we try to run Application_Start again next request
                throw new AppStartException(ex);
            }
        }

        private static void RegisterRoutes()
        {
            var serviceHostFactory = new AutofacWebServiceHostFactory();
            RouteTable.Routes.Add(new ServiceRoute("SeatService", new AutofacWebServiceHostFactory(), typeof(ISeatReservationForAircraftsWebService)));
        }

        private static void RegisterDependencies()
        {
            var registerDefinition = BuildRegisterDefinition();

            var registrator = new ReflectionRegistrator();

            //registrator.EnableVerboseTrace();

            registrator.RegisterDependencies(registerDefinition);
        }

        private static RegisterDefinition BuildRegisterDefinition()
        {
            return new RegisterDefinition
            {
                ExecutingAssembly = Assembly.GetExecutingAssembly(),
                ConfigurationLifetimeScope = LifetimeScopes.InstancePerLifetimeScope,
                DbContextLifetimeScope = LifetimeScopes.InstancePerLifetimeScope,
                InstanciateAllServicesAfterRegister = false,
                IgnoreTypes = new List<Type>(),
                InstanciateSingleInstanceServicesAfterRegister = true,
                DefaultServiceLifetimeScope = LifetimeScopes.InstancePerDependency,
                AdditionalEntryServices = new List<Type>(),
                //MaestrosSettingsDefinition = new MaestrosSettingsDefinition
                //{
                //    ApplicationId = ApplicationId,
                //    DataDirectoryPath = VuelingEnvironment.Current.GetDataDirectoryPath(),
                //    GenerateFromFileInCaseOfFailureOnLoad = true,
                //    TimeInMinutesToUpdate = 5,
                //    WriteInAppDataFile = true,
                //    NumOfRetriesInCaseOfFailureOnLoad = 5,
                //    TimeInSecondsBetweenRetries = 1,
                //}
            };
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception lastException = Server.GetLastError();
            if (lastException == null) return;
            Trace.TraceError(string.Format("Unhandled error. {0}", lastException));
            if (lastException.GetType() != typeof(AppStartException))
                Server.ClearError();
            else
                throw lastException;
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }
    }

    [Serializable]
    public class AppStartException : Exception
    {
        public AppStartException(Exception innerException)
            : base(innerException.Message, innerException)
        { }
    }
}
