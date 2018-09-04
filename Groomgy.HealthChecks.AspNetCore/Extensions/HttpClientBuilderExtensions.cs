using Microsoft.Extensions.DependencyInjection;
using Polly;
using System.Net.Http;

namespace Groomgy.HealthChecks.AspNetCore
{
    public static class HttpClientBuilderExtensions
    {
        public static IHttpClientBuilder AddPolicyHandlers(this IHttpClientBuilder builder, IAsyncPolicy<HttpResponseMessage>[] policies)
        {
            foreach (var policy in policies)
            {
                builder.AddPolicyHandler(policy);
            }
            return builder;
        }
    }
}
