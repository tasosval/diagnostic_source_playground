using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace DiagnosticSource.Console48
{
    internal class Program
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Program));
        
        public static async Task Main(string[] args)
        {
            // BasicConfigurator replaced with XmlConfigurator.
            XmlConfigurator.Configure(new System.IO.FileInfo("log4net.config"));
            
            log.Info("Log created");

            var app = new App();
            await app.Run();
        }
    }
}