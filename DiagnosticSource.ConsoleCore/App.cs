using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DiagnosticSourceTest.Lib;
using Microsoft.Extensions.Logging;

namespace DiagnosticSource.ConsoleCore
{
    public class App
    {
        private readonly ILogger<App> _logger;
        private readonly ITestService _testService;


        public App(ILogger<App> logger, ITestService testService, DiagnosticListenerToLogger diag): 
            this(logger, testService)
        { }
        
        public App(ILogger<App> logger, ITestService testService)
        {
            _logger = logger;
            _testService = testService;
        }

        public async Task Run()
        {
            await _testService.FetchItems("http://www.in.gr");
            await _testService.FetchItems("www.in.gr");
            
            _logger.LogInformation("finished execution");
        }
    }
}