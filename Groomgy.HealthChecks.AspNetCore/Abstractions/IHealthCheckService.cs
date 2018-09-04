using System.Threading.Tasks;

namespace Groomgy.HealthChecks.AspNetCore
{
    public interface IHealthCheckService
    {
        Task<CompositeHealthCheckResult> CheckAll();
    }
}
