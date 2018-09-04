using Groomgy.HealthChecks.AspNetCore.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Groomgy.HealthChecks.AspNetCore
{
    public static class HealthCheckBuilderExtensions
    {
        /// <summary>
        /// Checks if the tables specified are accessible on the database using the connection string given.
        /// </summary>
        /// <param name="builder">Health check builder</param>
        /// <param name="connectionString">Connection string to the Sqlite database</param>
        /// <param name="tables">Tables to check for existence</param>
        /// <returns></returns>
        public static IHealthCheckBuilder AddSqliteCheck(this IHealthCheckBuilder builder, string connectionString, params string[] tables) =>
            builder.Add(sp => new SqliteCheck(connectionString, tables, sp.GetRequiredService<ILogger<SqliteCheck>>()));
    }
}
