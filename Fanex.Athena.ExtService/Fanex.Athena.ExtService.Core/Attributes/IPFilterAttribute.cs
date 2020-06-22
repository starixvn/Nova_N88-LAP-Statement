using Fanex.Athena.ExtService.Core.Models.DTO;
using Fanex.Athena.ExtService.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using NetTools;
using System;
using System.Linq;
using System.Net;

namespace Fanex.Athena.ExtService.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class IPFilterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationOptions _applicationOptions;
        private readonly IClientService _clientService;

        public IPFilterAttribute(IOptions<ApplicationOptions> applicationOptionAccessor,
            IClientService clientService)
        {
            _applicationOptions = applicationOptionAccessor.Value;
            _clientService = clientService;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ipAddress = _clientService.RealClientIP;
            var whiteListIPList = _applicationOptions.WhiteList;

            var isInWhiteListIPList = !whiteListIPList.Any() || whiteListIPList.Any(_ => IsInRangeIP(_, ipAddress));
            if (!isInWhiteListIPList)
            {
                var result = new ContentResult
                {
                    Content = $"The IP Address: {ipAddress} was blocked"
                };

                context.Result = result;
            }
        }

        private static Func<string, IPAddress, bool> IsInRangeIP = (ipRangeString, ipAddress) =>
        {
            if (!IPAddressRange.TryParse(ipRangeString, out IPAddressRange ipRange))
            {
#pragma warning disable S112 // General exceptions should never be thrown
                throw new Exception($"The WhiteList IP: {ipRangeString} was invalid - client IP: ${ipAddress}");
#pragma warning restore S112 // General exceptions should never be thrown
            }
            return ipRange.Contains(ipAddress);
        };
    }
}