using Microsoft.Extensions.DependencyInjection;

namespace Groomgy.HealthChecks.AspNetCore
{
    public static class HealthCheckBuilderExtensions
    {
        public static IHealthCheckBuilder AddSelfCheck(this IHealthCheckBuilder builder, string message) =>
            builder.Add(sp => new SelfCheck(message));

        public static IHealthCheckBuilder AddUrlCheck(this IHealthCheckBuilder builder, string message, string url) =>
            builder.Add(sp => new UrlCheck(sp.GetRequiredService<IHealthCheckHttpClient>(), message, url));
    }
}
