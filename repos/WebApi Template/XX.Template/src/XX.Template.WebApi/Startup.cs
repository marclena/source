using ATC.HealthChecks.Extensions;
using ATC.HealthChecks.Extensions.Middleware;
using ATC.Log.Serilog.Impl.ServiceLibrary.Module;
using XX.Template.Library.Impl.Configuration;
using XX.Template.Repository.Impl.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ATC.Swagger.Extension;
using XX.Template.WebApi.Extensions.ServiceCollectionExtensions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc;

namespace XX.Template.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //Add app internal dependencies
            services.AddLibraryServices(Configuration)
                    .AddRepositoryServices(Configuration);             

            //Add external dependencies and nuget dependencies
            services.AddHealthCheckEndpoints(Configuration)
                    .AddLogWrapperDependencies()
                    .AddCustomSwagger()
                    .AddCustomHttpClients(Configuration);

            //If you want securing your API 
            //uncomment the following line of code and
            //complete the configuration section "JwtBearer" with the correct settings
            //services.AddCustomAuthentication(Configuration);

            //Add boilerplate dependencies
            services.AddCustomMvcCore();
            //services.AddMvc().AddMetrics();
            services.AddMvcCore().AddMetricsCore();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, 
            IHostingEnvironment env,
            IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UsePathBase(Configuration["PathBase"]);
            app.UseCustomSwagger(provider);

            app.AddHealthCheckApiMethod(new HealthCheckApiMethodMiddlewareOptions
                {
                    RelativeUri = Configuration["HealthCheckOkRelativeUri"]
                }
            );
            app.UseHealthAllEndpoints();
            //Uncomment the following lines of code
            //If you want securing your API
            //app.UseAuthentication();
            //app.AddJtiToLogContext();
            app.UseMvc();
        }
    }
}