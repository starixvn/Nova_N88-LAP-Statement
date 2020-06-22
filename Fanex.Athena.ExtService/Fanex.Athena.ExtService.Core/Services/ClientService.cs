using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Linq;
using System.Net;

namespace Fanex.Athena.ExtService.Core.Services
{
    public class ClientService : IClientService
    {
        private const string ForwardedForHeader = "X-Forwarded-For";

        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IPAddress RealClientIP
        {
            get
            {
                if (Context.Request.Headers.TryGetValue(ForwardedForHeader, out StringValues values)
                    && IPAddress.TryParse(values.FirstOrDefault(), out IPAddress address))
                {
                    return address;
                }
                return Context.Connection.RemoteIpAddress;
            }
        }

        public HttpContext Context => _httpContextAccessor.HttpContext;
    }
}