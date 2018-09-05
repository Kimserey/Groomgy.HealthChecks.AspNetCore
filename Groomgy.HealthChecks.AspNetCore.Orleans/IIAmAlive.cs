using Orleans;
using System.Threading.Tasks;

namespace Groomgy.HealthChecks.AspNetCore.Orleans
{
    public interface IIAmAlive : IGrainWithIntegerKey
    {
        Task<bool> Check();
    }
}
