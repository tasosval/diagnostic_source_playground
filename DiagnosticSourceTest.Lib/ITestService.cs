using System.Threading.Tasks;

namespace DiagnosticSourceTest.Lib
{
    public interface ITestService
    {
        Task<string> FetchItems(string url);
    }
}