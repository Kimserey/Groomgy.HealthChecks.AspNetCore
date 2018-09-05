using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Groomgy.HealthChecks.AspNetCore.WebB
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks(c =>
            {
                c.AddSelfCheck("WebB is running.");
            });
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
                Thread.Sleep(TimeSpan.FromSeconds(10));
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
