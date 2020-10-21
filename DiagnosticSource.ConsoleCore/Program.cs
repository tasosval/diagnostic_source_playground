using System;
using System.IO;
using DiagnosticSourceTest.Lib;
using DiagnosticSourceTest.Lib.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace DiagnosticSource.ConsoleCore
{
    public class Program
    {
        /// <summary>
        /// This is an Action to build the configuration that we will use for our application
        /// It adds any appsettings file and any environment variables
        /// </summary>
        private static Action<IConfigurationBuilder> BuildConfiguration =
            builder => builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        
        public static int Main(string[] args)
        {
            // Create and setup the configuration builder
            var builder = new ConfigurationBuilder();
            BuildConfiguration(builder);

            // We use the configuration to create the logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .CreateLogger();

            try
            {
                Log.Information("Getting the motors running...");
                // Run our application
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // Use the following line to only have the DiagnosticSource events (which must be handled)
                .AddDiagnosticSourceLogger()
                // .AddDiagnosticSourceLoggerDI()
                .UseSerilog()
                .ConfigureServices(ConfigureServices);

        private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
        {
            services
                .AddSingleton<ITestService, TestService>()
                // Use the following line to register the service that will listen to the DiagnosticSource events
                // and write them to the supplied logger
                .AddSingleton<DiagnosticListenerToLogger>()
                .AddScoped<App>()
                .AddHostedService<AppService>();
        }
    }
}