using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

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
        CancellationToken cancellationToken = default(CancellationToken))
        {
            IDictionary<string, object> result = new Dictionary<string, object>();

            await AddServerIPInfo(result);
            AddExtraServerInfo(result, _httpContextAccessor.HttpContext.Connection);

            return HealthCheckResult.Healthy("Server Info", new ReadOnlyDictionary<string, object>(result));
        }

        private static async Task AddServerIPInfo(IDictionary<string, object> result)
        {
            var hostName = Dns.GetHostName();
            result.Add("Host Name", hostName);

            var serverIps = await Dns.GetHostAddressesAsync(hostName).ConfigureAwait(false);
            for (var i = 1; i < serverIps.Length; i++)
            {
                if (!serverIps[i].Equals("::1"))
                {
                    result.Add($"IP{i}", serverIps[i].ToString());
                }
            }
        }

        private static void AddExtraServerInfo(IDictionary<string, object> result, ConnectionInfo connectionInfo)
        {
            result.Add("Local Ip", connectionInfo.LocalIpAddress.ToString());
            result.Add("Local Port", connectionInfo.LocalPort);
            result.Add("Client Ip", connectionInfo.RemoteIpAddress.ToString());
        }
    }
}