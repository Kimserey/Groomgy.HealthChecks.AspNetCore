using System.Net.Http;
using System.Threading.Tasks;

namespace Groomgy.HealthChecks.AspNetCore
{
    public interface IHealthCheckHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string url);
    }
}
