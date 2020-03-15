using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Fanex.Athena.ExtService.Core.Middleware
{
    public class WhoAmIHealthCheck : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default(CancellationToken))
        {
            var ip = await System.Net.Dns.GetHostAddressesAsync(
                       System.Net.Dns.GetHostName()).ConfigureAwait(false);
            IDictionary<string, object> result = new Dictionary<string, object>();
            for (var i = 1; i < ip.Length; i++)
            {
                if (!ip[i].Equals("::1"))
                {
                    result.Add($"IP{i}", ip[i].ToString());
                }
            }
            return HealthCheckResult.Healthy("Server Info", new ReadOnlyDictionary<string, object>(result));

        }


    }
}
