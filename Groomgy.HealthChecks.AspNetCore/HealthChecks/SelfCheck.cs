using System.Threading.Tasks;

namespace Groomgy.HealthChecks.AspNetCore
{
    public class SelfCheck : IHealthCheck
    {
        private string _name;
        private string _message;

        public SelfCheck(string message)
        {
            _name = typeof(SelfCheck).Name;
            _message = message;
        }

        public Task<HealthCheckResult> Check()
        {
            return Task.FromResult(new HealthCheckResult
            {
                Name = _name,
                Message = _message,
                Status = CheckStatus.Healthy
            });
        }
    }
}
