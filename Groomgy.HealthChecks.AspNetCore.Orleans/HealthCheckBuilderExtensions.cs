using Groomgy.HealthChecks.AspNetCore.Orleans;
using Microsoft.Extensions.DependencyInjection;
using Orleans;

namespace Groomgy.HealthChecks.AspNetCore
{
    public static class HealthCheckBuilderExtensions
    {
        public static IHealthCheckBuilder AddOrleansClientCheck(this IHealthCheckBuilder builder, string clusterId) =>
            builder.Add(sp => new OrleansClientCheck(() => sp.GetRequiredService<IGrainFactory>()));
    }
}
