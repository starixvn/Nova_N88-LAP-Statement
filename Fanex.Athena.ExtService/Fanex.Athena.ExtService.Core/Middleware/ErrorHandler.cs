using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using System;
using System.Threading.Tasks;

namespace Fanex.Athena.ExtService.Core.Middleware
{
    public class ErrorHandler
    {
        private const int ServerErrorCode = 500;
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandler> _logger;

        public ErrorHandler(RequestDelegate next, ILogger<ErrorHandler> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;

            if (error != null && error.Error != null)
            {
                await HandleError(context, error.Error);
            }

            try
            {
                await _next.Invoke(context);
            }
            catch (Exception exception)
            {
                await HandleError(context, exception);
            }
        }

        private async Task HandleError(HttpContext context, Exception error)
        {
            context.Response.StatusCode = ServerErrorCode;
            context.Response.ContentType = "application/json";
            var errorMsg = $"Msg={error.Message}\n Stacktrace={error.StackTrace}" +
                $"\n Path={context.Request.Path}\n Query={context.Request.QueryString}";
            _logger.LogCritical(error, errorMsg);
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                State = "Internal Server Error",
                Msg = "Sorry, an error occurred while processing your request."
            }));
        }
    }


}
