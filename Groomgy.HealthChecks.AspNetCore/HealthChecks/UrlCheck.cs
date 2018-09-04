using System;
using System.Net;
using System.Threading.Tasks;

namespace Groomgy.HealthChecks.AspNetCore
{
    public class UrlCheck : IHealthCheck
    {
        private readonly string _name;
        private readonly IHealthCheckHttpClient _httpClient;
        private readonly string _message;
        private readonly string _url;

        public UrlCheck(IHealthCheckHttpClient httpClient, string message, string url)
        {
            _name = typeof(UrlCheck).Name;
            _httpClient = httpClient;
            _message = message;
            _url = url;
        }

        public async Task<HealthCheckResult> Check()
        {
            try
            {
                var response = await _httpClient.GetAsync(_url).ConfigureAwait(false);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return new HealthCheckResult
                    {
                        Name = _name,
                        Message = $"Successfully contacted url '{_url}'. Message: " + _message,
                        Status = CheckStatus.Healthy
                    };
                }

                return new HealthCheckResult
                {
                    Name = _name,
                    Message = $"Failed to contact url '{_url}'. Message: " + await response.Content.ReadAsStringAsync(),
                    Status = CheckStatus.Unhealthy
                };

            }
            catch (Exception ex)
            {
                return new HealthCheckResult
                {
                    Name = _name,
                    Message = $"Failed to contact url '{_url}'. Message: " + ex.Message,
                    Status = CheckStatus.Unhealthy
                };
            }
        }
    }
}
