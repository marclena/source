using Swashbuckle.Application;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Vueling.XXX.WebApi.App_Start
{
    public static class SwaggerConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            
            //config
            GlobalConfiguration.Configuration
                .EnableSwagger("docs/{apiVersion}/swagger", c =>
                {
                    c.UseFullTypeNameInSchemaIds();

                    c.MultipleApiVersions(
                        (apiDescription, version) => true,
                        info =>
                        {
                            //foreach (var group in apiExplorer.ApiDescriptions)
                            //{

                            //}
                        }
                        );

                    //c.SingleApiVersion("v1", "Vueling XXX WebApi")
                    //    .Description("This API contains information about Vueling XXX WebApi.")
                    //    .Contact(cc => cc.Name("Vueling"));

                    c.IncludeXmlComments(string.Format("{0}/bin/Vueling.WebApi.xml", AppDomain.CurrentDomain.BaseDirectory));

                    c.DescribeAllEnumsAsStrings();
                    c.IgnoreObsoleteProperties();

                    c.MapType<int?>(() => new Schema { type = "integer, null", format = "int32", @default = 0 });
                    c.MapType<decimal?>(() => new Schema { type = "decimal, null", format = "double", @default = 0.0 });
                    c.MapType<bool?>(() => new Schema { type = "boolean, null", @default = false });
                    c.MapType<TimeSpan?>(() => new Schema { type = "string, null", @default = "00:00:00" });
                    c.MapType<TimeSpan>(() => new Schema { type = "string, null", @default = "00:00:00" });
                    c.MapType<byte>(() => new Schema { type = "byte", @default = 0 });
                    c.MapType<byte?>(() => new Schema { type = "byte, null", @default = 0 });


                    //Authentication with oauth2 in swagger

                    //c.OAuth2("oauth2")
                    //    .Description("Identity Server 3 Implicit Grant")
                    //    .Flow("implicit")
                    //    .AuthorizationUrl("https://authorizationserver/authorize")
                    //    .TokenUrl("https://authorizationserver/token")
                    //    .Scopes(scopes =>
                    //    {
                    //        scopes.Add("SCOPE_A", "Access to all Vueling.XXX.WebAPI resources");
                    //    });

                    c.OperationFilter<AssignOAuth2SecurityRequirements>();
                    c.OperationFilter<MultipartResponseType>();
                    c.OperationFilter(() => new AddRequiredHeaderParameter());

                    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.Last());

                })
                .EnableSwaggerUi(c =>
                {
                    c.DisableValidator();
                    c.InjectStylesheet(thisAssembly, "Vueling.XXX.WebAPI.Content.Swagger.css");
                    c.CustomAsset("VuelingLogo", thisAssembly, "Vueling.XXX.WebAPI.Content.Images.VuelingLogo.png");
                    c.EnableOAuth2Support("bff-core-swagger-ui", "swagger-realm", "Vueling.XXX.WebApi Swagger UI");
                });
        }
    }

    public class MultipartResponseType : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.operationId == "Multipart")
            {
                operation.consumes.Add("multipart/form-data");
                operation.produces.Add("multipart/form-data");

                operation.parameters.Add(new Parameter
                {
                    name = "file",
                    required = true,
                    type = "file",
                    @in = "formData"
                });
            }
        }
    }

    public class AddRequiredHeaderParameter : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            if (operation.parameters == null)
                operation.parameters = new List<Parameter>();

            operation.parameters.Add(new Parameter
            {
                name = "Authorization",
                @in = "header",
                type = "string",
                required = false,
                description = "Bearer {Token}"
            });
        }
    }

    public class AssignOAuth2SecurityRequirements : IOperationFilter
    {
        public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
        {
            // Determine if the controller has the Authorize attribute
            var authorizeAttributes = apiDescription.ActionDescriptor.ControllerDescriptor.GetCustomAttributes<AuthorizeAttribute>();

            if (!authorizeAttributes.Any())
                return;

            // Correspond each "Authorize" role to an oauth2 scope
            //var scopes =
            //    authorizeAttributes
            //    .SelectMany(attr => attr.Roles.Split(','))
            //    .Distinct()
            //    .ToList();

            // Initialize the operation.security property if it hasn't already been
            //operation.security = operation.security ?? new List<IDictionary<string, IEnumerable<string>>>();

            //SETS IF SOME SCOPE IS REQUIRED

            //var oAuthRequirements = new Dictionary<string, IEnumerable<string>>
            //    {
            //        //{ "oauth2", scopes }
            //        { "oauth2", new List<string> { "SCOPE1" }}
            //    };

            //operation.security.Add(oAuthRequirements);
        }
    }
}