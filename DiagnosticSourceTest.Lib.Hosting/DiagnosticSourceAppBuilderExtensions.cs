using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DiagnosticSourceTest.Lib.Hosting
{
    public static class DiagnosticSourceAppBuilderExtensions
    {
        public static IServiceCollection AddDiagnosticSourceLogger(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IDiagnosticSourceLogger, DiagnosticSourceLogger>();
            return serviceCollection;
        }

        /// <summary>
        /// Register a DiagnosticSourceLogger that will use the ILogger to log events
        /// meaning that we only need to set up the <see cref="ILogger{DiagnosticSourceLogger}"/>
        ///
        /// The responsibility of logging is with <see cref="DiagnosticSourceLogger"/> 
        /// </summary>
        public static IHostBuilder AddDiagnosticSourceLoggerDI(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((_, serviceCollection) =>
            {
                serviceCollection.AddSingleton<IDiagnosticSourceLogger, DiagnosticSourceLogger>();
            });
            return hostBuilder;
        }
        
        /// <summary>
        /// Register a DiagnosticSourceLogger that will use the DiagnosticSource to create events
        /// meaning that we will need to subscribe to them, in order to do anything with them
        ///
        /// The responsibility stays with the calling application 
        /// </summary>
        public static IHostBuilder AddDiagnosticSourceLogger(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((_, serviceCollection) =>
            {
                serviceCollection.AddSingleton<IDiagnosticSourceLogger, DiagnosticSourceLogger>(
                    provider => new DiagnosticSourceLogger());
            });
            return hostBuilder;
        }
    }
}