using Microsoft.Web.Http;
using Microsoft.Web.Http.Routing;
using System.Web.Http;
using System.Web.Http.Routing;
using Vueling.XXX.WebApi.Handlers;

namespace Vueling.XXX.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var constraintResolver = new DefaultInlineConstraintResolver()
            {
                ConstraintMap = { ["apiVersion"] = typeof(ApiVersionRouteConstraint) }
            };

            config.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.DefaultApiVersion = ApiVersion.Default;
            });

            config.MapHttpAttributeRoutes(constraintResolver);

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "v{version:apiVersion}/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

           GlobalConfiguration.Configuration.MessageHandlers.Add(new LoggingHandler());
        }
    }
}
