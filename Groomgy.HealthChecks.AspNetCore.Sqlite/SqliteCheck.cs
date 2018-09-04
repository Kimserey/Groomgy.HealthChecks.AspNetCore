using Microsoft.Extensions.Logging;
using SQLite;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Groomgy.HealthChecks.AspNetCore.Sqlite
{
    public class SqliteCheck : IHealthCheck
    {
        private readonly string _name;
        private readonly string _connectionString;
        private readonly string[] _tables;
        private readonly ILogger<SqliteCheck> _logger;

        public SqliteCheck(string connectionString, string[] tables, ILogger<SqliteCheck> logger)
        {
            _name = typeof(SqliteCheck).Name;
            _connectionString = connectionString;
            _tables = tables;
            _logger = logger;
        }

        public Task<HealthCheckResult> Check()
        {
            try
            {
                var path = _connectionString.Remove(0, "Date Source=".Length);

                if (File.Exists(path))
                {
                    using (var conn = new SQLiteConnection(path))
                    {
                        var count = _tables.Aggregate(0, (res, t) => res + conn.CreateCommand($"SELECT COUNT(name) FROM sqlite_master WHERE type='table' AND name='{t}'").ExecuteScalar<int>());
                        if (count == _tables.Length)
                        {
                            return Task.FromResult(new HealthCheckResult
                            {
                                Name = _name,
                                Status = CheckStatus.Healthy,
                                Message = $"Successfully found all table(s) on Sqlite database 'REDACTED (length {_connectionString.Length})'. Tables: {string.Join(", ", _tables)}"
                            });
                        }
                        else
                        {
                            return Task.FromResult(new HealthCheckResult
                            {
                                Name = _name,
                                Status = CheckStatus.Unhealthy,
                                Message = $"Failed to find all tables on Sqlite database 'REDACTED (length {_connectionString.Length})'. Tables: {string.Join(", ", _tables)}"
                            });
                        }
                    }
                }
                else
                {
                    return Task.FromResult(new HealthCheckResult
                    {
                        Name = _name,
                        Status = CheckStatus.Unhealthy,
                        Message = $"Failed to find Sqlite database at 'REDACTED (length {_connectionString.Length})'."
                    });
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return Task.FromResult(new HealthCheckResult
                {
                    Name = _name,
                    Status = CheckStatus.Unhealthy,
                    Message = $"Failed to execute command healcheck command on database for path 'REDACTED (length {_connectionString.Length})'. Message:{ex.Message}"
                });
            }
        }
    }
}
