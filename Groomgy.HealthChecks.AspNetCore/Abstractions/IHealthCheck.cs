using System.Threading.Tasks;

namespace Groomgy.HealthChecks.AspNetCore
{
    public interface IHealthCheck
    {
        Task<HealthCheckResult> Check();
    }
}
