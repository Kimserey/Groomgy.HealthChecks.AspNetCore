using System;
using System.Collections.Generic;

namespace Groomgy.HealthChecks.AspNetCore.Implementations
{
    public class HealthCheckBuilder : IHealthCheckBuilder
    {
        private List<Func<IServiceProvider, IHealthCheck>> _factories = new List<Func<IServiceProvider, IHealthCheck>>();

        public IHealthCheckBuilder Add(Func<IServiceProvider, IHealthCheck> factory)
        {
            _factories.Add(factory);
            return this;
        }

        public IEnumerable<Func<IServiceProvider, IHealthCheck>> GetAll()
        {
            return _factories;
        }
    }
}
