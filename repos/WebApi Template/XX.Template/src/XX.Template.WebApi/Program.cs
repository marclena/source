using System.IO;
using ATC.Log.Serilog.Impl.ServiceLibrary.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using App.Metrics;
using App.Metrics.AspNetCore;
using System;
using App.Metrics.Filtering;
using App.Metrics.Reporting.Elasticsearch.Client;
using App.Metrics.Formatters.Json;

namespace XX.Template.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
           
            var filter = new MetricsFilter();

            filter.WhereContext(c => c == MetricsRegistry.Context);
            var webHostBuilder = new WebHostBuilder()
                
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFiles(hostingContext);
                    config.AddEnvironmentVariables();
                    if (args != null)
                        config.AddCommandLine(args);
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    Log.Logger = new LoggerConfiguration()
                        .AddDefaultSerilogEnrichers(hostingContext.Configuration)
                        .ReadFrom.Configuration(hostingContext.Configuration)
                        .CreateLogger();
                })
                 .ConfigureMetricsWithDefaults(builder =>
                 {
                     
                     builder.Report.ToTextFile(@"C:\influx\metrics2.txt", TimeSpan.FromSeconds(20));
                     builder.Report.ToInfluxDb("http://127.0.0.1:8086", "metrics", TimeSpan.FromSeconds(1));


                 })
                 
                .UseMetricsWebTracking(options =>
                {
                    options.ApdexTrackingEnabled = true;
                })
                //.ConfigureMetricsWithDefaults(
                //builder =>
                //{
                //    builder.Filter.With(filter);
                //    builder.Report.ToConsole(TimeSpan.FromSeconds(2));
                //    builder.Report.ToTextFile(@"C:\influx\metrics1.txt", TimeSpan.FromSeconds(20));
                //    builder.Report.ToInfluxDb("http://localhpst:8086", "appmetricsreservoirs", TimeSpan.FromSeconds(1));

                //})
                .UseMetrics()
                .UseMetricsWebTracking()
                .UseSerilog()
                .UseKestrel()
                .UseIISIntegration()
                .UseStartup<Startup>();

            return webHostBuilder;
        }
    }
}