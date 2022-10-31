using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Fanex.Athena.ExtService.Core.Middleware
{
    public class WhoAmIHealthCheck : IHealthCheck
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WhoAmIHealthCheck(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
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
            result.Add("Client Ip", _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString());

            return HealthCheckResult.Healthy("Server Info", new ReadOnlyDictionary<string, object>(result));
        }
    }
}