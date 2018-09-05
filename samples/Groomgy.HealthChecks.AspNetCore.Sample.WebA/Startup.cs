using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Groomgy.HealthChecks.AspNetCore.Implementations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Groomgy.HealthChecks.AspNetCore.Sample.WebA
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks(c =>
            {
                c.AddSelfCheck("WebA is running.");
                c.AddUrlCheck("WebB is accessible.", "http://localhost:5001");
            },
            new[] { PolicyHandler.Timeout(1) });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecksEndpoint();

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
