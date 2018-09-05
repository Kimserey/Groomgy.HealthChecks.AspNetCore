using Orleans;
using System;
using System.Threading.Tasks;

namespace Groomgy.HealthChecks.AspNetCore.Orleans
{
    public class OrleansClientCheck : IHealthCheck
    {
        private readonly string _name;
        private readonly Func<IGrainFactory> _factory;

        public OrleansClientCheck(Func<IGrainFactory> factory)
        {
            _name = typeof(OrleansClientCheck).Name;
            _factory = factory;
        }

        public async Task<HealthCheckResult> Check()
        {
            try
            {
                var grain = _factory().GetGrain<IIAmAlive>(0);
                var result = await grain.Check();
                if (result)
                {
                    return new HealthCheckResult
                    {
                        Name = _name,
                        Status = CheckStatus.Healthy,
                        Message = $"Successfully accessed Orleans Cluster."
                    };
                }
                else
                {
                    return new HealthCheckResult
                    {
                        Name = _name,
                        Status = CheckStatus.Unhealthy,
                        Message = $"Failed to access Orleans Cluster. IAmAlive grain check returned failed state."
                    };
                }
            }
            catch (Exception ex)
            {
                return new HealthCheckResult
                {
                    Name = _name,
                    Status = CheckStatus.Unhealthy,
                    Message = $"Failed to access Orleans Cluster. Message: " + ex.Message
                };
            }
        }
    }
}
