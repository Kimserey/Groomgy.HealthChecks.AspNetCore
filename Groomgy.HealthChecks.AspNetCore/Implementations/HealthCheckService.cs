using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Groomgy.HealthChecks.AspNetCore.Implementations
{
    public class HealthCheckService : IHealthCheckService
    {
        private IEnumerable<IHealthCheck> _checks;

        public HealthCheckService(IEnumerable<IHealthCheck> checks)
        {
            _checks = checks;
        }

        public async Task<CompositeHealthCheckResult> CheckAll()
        {
            return new CompositeHealthCheckResult
            {
                Results = await Task.WhenAll(_checks.Select(async c => await c.Check()))
            };
        }
    }
}
