using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Groomgy.HealthChecks.AspNetCore
{
    public class HealthCheckHttpClient: IHealthCheckHttpClient
    {
        private HttpClient _httpClient;

        public HealthCheckHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<HttpResponseMessage> GetAsync(string url)
        {
            _httpClient.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue { NoCache = true };
            return _httpClient.GetAsync(url);
        }
    }
}
