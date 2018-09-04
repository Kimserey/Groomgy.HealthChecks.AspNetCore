using System.Collections.Generic;
using System.Linq;

namespace Groomgy.HealthChecks.AspNetCore
{
    public class CompositeHealthCheckResult
    {
        public static CompositeHealthCheckResult Failing(string error)
        {
            return new CompositeHealthCheckResult
            {
                Results = new[] {
                    new HealthCheckResult {
                        Status = CheckStatus.Unhealthy,
                        Name = "Failing health check",
                        Message = "Failed health check. Message: " + error
                    }
                }
            };
        }

        public IEnumerable<HealthCheckResult> Results { get; set; }

        public CheckStatus CompositeStatus => Results.Any(r => r.Status == CheckStatus.Unhealthy) ? CheckStatus.Unhealthy : CheckStatus.Healthy;
    }
}
