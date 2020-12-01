using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Routing;
using System.Reflection;
using Vueling.Configuration.Library;
using Vueling.DIRegister.AssemblyDiscovery.ServiceLibrary;
using Vueling.DIRegister.AssemblyDiscovery.ServiceLibrary.DTO;
using Vueling.Web.UI.Library;
using Vueling.XXX.WebUI.Helpers;
using Vueling.Web.Library.Library.MVC;
using Vueling.Web.Library;
using log4net;
using System.Diagnostics;
using Autofac;
using Autofac.Integration.Mvc;
using Vueling.Web.UI.Library.Configuration;
using Vueling.Web.UI.Library.Controllers;
using System.Globalization;

namespace Vueling.XXX.WebUI
{
    public class MvcApplication : HttpApplication
    {
        public const string ApplicationId = "Vueling.XXX.WebUI";

        static readonly ILog Logger = LogManager.GetLogger(typeof(MvcApplication));

        public static void RegisterModelBinders()
        {
            RegisterGlobalizedModelBinderFor<DateTime>(default(DateTime));
            RegisterGlobalizedModelBinderFor<DateTime?>(default(DateTime));
            RegisterGlobalizedModelBinderFor<Decimal>(default(Decimal));
            RegisterGlobalizedModelBinderFor<Decimal?>(default(Decimal));
        }

        public static void RegisterGlobalizedModelBinderFor<T>(T sameAsTypeParameter)
        {
            ModelBinders.Binders.Add(sameAsTypeParameter.GetType(), new CurrentUICultureModelBinder<T>());
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LoggingFilterAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("SSO/SSO.ashx");
            routes.IgnoreRoute("{*favicon}", new { favicon = @"(./)?favicon.ico(/.)?" });
            routes.IgnoreRoute("{*allaspx}", new { allaspx = @".*\.aspx(/.*)?" });
            routes.IgnoreRoute("{*alljs}", new { alljs = @".*\.js(/.*)?" });

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            try
            {
                VuelingEnvironment.InitializeCurrentForApplication(ApplicationId);

                HostingEnvironment.RegisterVirtualPathProvider(new EmbeddedVirtualPathProvider());

                AreaRegistration.RegisterAllAreas();
                RegisterGlobalFilters(GlobalFilters.Filters);
                RegisterRoutes(RouteTable.Routes);

                RegisterDependencies();
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                HttpRuntime.UnloadAppDomain(); // Make sure we try to run Application_Start again next request
                throw new AppStartException(ex);
            }
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
                DbContextLifetimeScope = LifetimeScopes.InstancePerDependency,
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

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception exception = Server.GetLastError();

            if (exception == null) { return; }

            if (exception.GetType() == typeof(AppStartException)) { throw exception; }

            Server.ClearError();

            if (HttpContext.Current == null || AutofacDependencyResolver.Current == null)
            {
                // errors in Application_Start will end up here   
                Logger.Fatal("Application could not be started.", exception);
            }
            else
            {
                LogException(exception);

                PrintUnHandledExceptions(exception);
            }
        }

        private void LogException(Exception exception)
        {
            if (exception.Message.Contains("File does not exist"))
            {
                Logger.Error(string.Format(CultureInfo.InvariantCulture, "File does not exist. Requested url: {0}", Request.RawUrl));
            }
            else
            {
                Logger.Error(exception);
            }
        }

        private void PrintUnHandledExceptions(Exception exception)
        {
            var routeData = new RouteData();
            routeData.Values["controller"] = "Errors";
            routeData.Values["action"] = "UnHandled";
            routeData.Values["exception"] = exception;

            var webUILibraryConfiguration = AutofacDependencyResolver.Current.ApplicationContainer.Resolve<IWebUILibraryConfiguration>();
            IController errorsController = new ErrorsController(webUILibraryConfiguration);
            var rc = new RequestContext(new HttpContextWrapper(Context), routeData);
            errorsController.Execute(rc);
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