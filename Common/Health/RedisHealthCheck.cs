using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Health
{
    public class RedisHealthCheck : IHealthCheck
    {
        private readonly string _configuration;

        public RedisHealthCheck(string configuration)
        {
            _configuration = configuration;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var connection = await ConnectionMultiplexer.ConnectAsync(_configuration);
                return HealthCheckResult.Healthy();
            }
            catch (Exception)
            {
                return HealthCheckResult.Unhealthy();
            }
        }
    }
}
