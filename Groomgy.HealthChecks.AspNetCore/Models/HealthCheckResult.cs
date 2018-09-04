namespace Groomgy.HealthChecks.AspNetCore
{
    public class HealthCheckResult
    {
        public string Name { get; set; }

        public CheckStatus Status { get; set; }

        public string Message { get; set; }
    }
}
