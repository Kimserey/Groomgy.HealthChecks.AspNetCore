using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;

namespace Groomgy.HealthChecks.AspNetCore
{
    public class HealthCheckMiddleware
    {
        private RequestDelegate _next;

        public HealthCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IConfiguration configuration, IHealthCheckService healthCheckService)
        {
            var endpoint = configuration["endpoints:healthcheck"];

            if (string.IsNullOrWhiteSpace(endpoint))
            {
                endpoint = "/health";
            }

            if (context.Request.Path.Equals(endpoint) && context.Request.Method == HttpMethods.Get)
            {
                var result = await healthCheckService.CheckAll();

                if (result.CompositeStatus != CheckStatus.Healthy)
                    context.Response.StatusCode = 503;

                context.Response.Headers.Add("content-type", "application/json");
                await context.Response.WriteAsync(
                    JsonConvert.SerializeObject(
                        result,
                        new JsonSerializerSettings {
                            Converters = { new StringEnumConverter() },
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        }
                    )
                );
                return;
            }

            await _next(context);
        }
    }
}
