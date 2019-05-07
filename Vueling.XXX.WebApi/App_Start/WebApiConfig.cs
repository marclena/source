using Microsoft.Web.Http.Routing;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using System.Web.Http.Routing;
using Vueling.XXX.WebApi.Handlers;
using Vueling.XXX.WebApi.Logging;

namespace Vueling.XXX.WebApi
{
    /// <summary>
    /// 
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public static void Register(HttpConfiguration configuration)
        {
            configuration.Services.Replace(typeof(IExceptionLogger), new UnhandledExceptionLogger());

            var constraintResolver = new DefaultInlineConstraintResolver() { ConstraintMap = { ["apiVersion"] = typeof(ApiVersionRouteConstraint) } };

            // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
            configuration.AddApiVersioning(options => options.ReportApiVersions = true);
            configuration.MapHttpAttributeRoutes(constraintResolver);

            // add the versioned IApiExplorer and capture the strongly-typed implementation (e.g. VersionedApiExplorer vs IApiExplorer)
            // note: the specified format code will format the version as "'v'major[.minor][-status]"
            var apiExplorer = configuration.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";
                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });
        }
    }
}
