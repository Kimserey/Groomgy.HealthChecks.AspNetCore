using System;
using System.Collections.Generic;

namespace Groomgy.HealthChecks.AspNetCore
{
    public interface IHealthCheckBuilder
    {
        IHealthCheckBuilder Add(Func<IServiceProvider, IHealthCheck> factory);

        IEnumerable<Func<IServiceProvider, IHealthCheck>> GetAll();
    }
}
