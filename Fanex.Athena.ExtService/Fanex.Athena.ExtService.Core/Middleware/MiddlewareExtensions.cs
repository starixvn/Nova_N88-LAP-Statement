using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Fanex.Athena.ExtService.Core.Middleware
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseJWTokenOnPage(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JWTokenHandler>();
        }

        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorHandler>();
        }

        public static IApplicationBuilder UseCustomHealthCheck(this IApplicationBuilder builder)
        {
            return builder.UseHealthChecks("/health", new HealthCheckOptions
            {
                AllowCachingResponses = false,
                Predicate = _ => true,
                ResponseWriter = async (context, result) =>
                {
                    context.Response.ContentType = "application/json";
                    var json = new JObject(
                         new JProperty("status", result.Status.ToString()),
                         new JProperty("results", new JObject(result.Entries.Select(pair =>
                             new JProperty(pair.Key, new JObject(
                                 new JProperty("status", pair.Value.Status.ToString()),
                                 new JProperty("description", pair.Value.Description),
                                 new JProperty("data", new JObject(pair.Value.Data.Select(
                                     p => new JProperty(p.Key, p.Value))))))))));
                    await context.Response.WriteAsync(
                        json.ToString(Formatting.Indented));
                }
            });
        }
    }
}