using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiagnosticSourceTest.Lib.Hosting
{
    public static class DiagnosticSourceAppBuilderExtensions
    {
        public static IServiceCollection AddDiagnosticSourceLogger(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IDiagnosticSourceLogger, DiagnosticSourceLogger>();
            return serviceCollection;
        }

        public static IHostBuilder AddDiagnosticSourceLoggerDI(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((_, serviceCollection) =>
            {
                serviceCollection.AddSingleton<IDiagnosticSourceLogger, DiagnosticSourceLogger>();
            });
            return hostBuilder;
        }
        
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