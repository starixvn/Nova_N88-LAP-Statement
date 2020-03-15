using System;
using System.Linq;
using System.Net;

using Fanex.Athena.ExtService.Core.Models.DTO;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Fanex.Athena.ExtService.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class IPFilterAttribute : ActionFilterAttribute
    {
        private readonly ApplicationOptions _applicationOptions;

        public IPFilterAttribute(IOptions<ApplicationOptions> applicationOptionAccessor)
        {
            _applicationOptions = applicationOptionAccessor.Value;

        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ipAddress = context.HttpContext.Connection.RemoteIpAddress;
            var whiteListIPList = _applicationOptions.WhiteList;

            var isInWhiteListIPList = !whiteListIPList.Any() || whiteListIPList.Any(x => IPAddress.Parse(x).Equals(ipAddress));
            if (!isInWhiteListIPList)
            {
                var result = new ContentResult
                {
                    Content = $"The IP Address: {ipAddress} was blocked"
                };

                context.Result = result;
            }
        }
    }
}
