using Orleans;
using System;
using System.Threading.Tasks;

namespace Groomgy.HealthChecks.AspNetCore.Orleans
{
    public class OrleansClientCheck : IHealthCheck
    {
        private readonly string _name;
        private readonly Func<IGrainFactory> _factory;
        private readonly string _clusterId;
        private readonly Action<string> _onError;

        public OrleansClientCheck(Func<IGrainFactory> factory, string clusterId, Action<string> onError = null)
        {
            _name = typeof(OrleansClientCheck).Name;
            _factory = factory;
            _clusterId = clusterId;
            _onError = onError;
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
                        Message = $"Successfully accessed Orleans Cluster '{_clusterId}' from client."
                    };
                }
                else
                {
                    return new HealthCheckResult
                    {
                        Name = _name,
                        Status = CheckStatus.Unhealthy,
                        Message = $"Failed to access Orleans Cluster '{_clusterId}' from client. IAmAlive grain check returned failed state."
                    };
                }
            }
            catch (Exception ex)
            {
                return new HealthCheckResult
                {
                    Name = _name,
                    Status = CheckStatus.Unhealthy,
                    Message = $"Failed to access Orleans Cluster '{_clusterId}' from client. Message: " + ex.Message
                };
            }
        }
    }
}
