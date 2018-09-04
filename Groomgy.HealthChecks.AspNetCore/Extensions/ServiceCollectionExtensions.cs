using Groomgy.HealthChecks.AspNetCore.Implementations;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using System;
using System.Net.Http;

namespace Groomgy.HealthChecks.AspNetCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddHealthChecks(this IServiceCollection services, Action<IHealthCheckBuilder> configureHealthCheckBuilder, IAsyncPolicy<HttpResponseMessage>[] policies)
        {
            if (configureHealthCheckBuilder == null)
            {
                throw new ArgumentNullException(nameof(configureHealthCheckBuilder));
            }

            if (policies == null)
            {
                throw new ArgumentNullException(nameof(policies));
            }

            var httpClient = services
                .AddHttpClient<IHealthCheckHttpClient, HealthCheckHttpClient>()
                .AddPolicyHandlers(policies);

            services.AddTransient<IHealthCheckService, HealthCheckService>();

            var builder = new HealthCheckBuilder();
            configureHealthCheckBuilder(builder);

            foreach (var factory in builder.GetAll())
            {
                services.AddTransient(sp => factory(sp));
            }

            return services;
        }

        public static IServiceCollection AddHealthChecks(this IServiceCollection services, Action<IHealthCheckBuilder> configureHealthCheckBuilder)
        {
            return AddHealthChecks(services, configureHealthCheckBuilder, new []{ PolicyHandler.WaitAndRetry(2), PolicyHandler.Timeout() });
        }
    }
}
