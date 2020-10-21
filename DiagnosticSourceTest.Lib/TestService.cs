using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace DiagnosticSourceTest.Lib
{
    public class TestService: ITestService
    {
        private readonly IDiagnosticSourceLogger _logger;
        private readonly HttpClient _client;

        public TestService(IDiagnosticSourceLogger logger)
        {
            _logger = logger;
            _client = new HttpClient();
        }
        
        public async Task<string> FetchItems(string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                // log this error with diagnosticSource
                _logger.Log("FetchFailure", new {Url = url, Message = "Invalid url"});
                return "";
            }
            
            var response = await _client.GetAsync(new Uri(url));
            if (response.IsSuccessStatusCode)
            {
                _logger.Log("FetchSuccessful", new {Url=url, Response=response.StatusCode});
                return await response.Content.ReadAsStringAsync();
            }

            // log this error with diagnosticSource
            _logger.Log("FetchFailure", new {Url = url, Message = "Erroneous response"});
            return "";
        }
    }
}