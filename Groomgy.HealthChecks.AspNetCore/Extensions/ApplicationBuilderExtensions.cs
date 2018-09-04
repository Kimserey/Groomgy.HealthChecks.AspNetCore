using Microsoft.AspNetCore.Builder;

namespace Groomgy.HealthChecks.AspNetCore
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseHealthChecksEndpoint(this IApplicationBuilder app) =>
            app.UseMiddleware<HealthCheckMiddleware>();
    }
}
