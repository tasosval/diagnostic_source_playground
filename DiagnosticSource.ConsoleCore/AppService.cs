using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace DiagnosticSource.ConsoleCore
{
    internal class AppService: BackgroundService
    {
        private readonly App _app;

        public AppService(App app)
        {
            _app = app ?? throw new ArgumentNullException(nameof(app));
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _app.Run();
        }
    }
}