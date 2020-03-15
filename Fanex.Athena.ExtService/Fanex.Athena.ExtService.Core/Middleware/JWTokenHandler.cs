using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Fanex.Athena.ExtService.Core.Middleware
{
    public class JWTokenHandler
    {
        private readonly RequestDelegate _next;
        const string HeaderAuthKey = "Authorization";
        const string SuffixTokenValue = "Bearer ";

        public JWTokenHandler(RequestDelegate next)
        {
            _next = next;
        }
        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey(HeaderAuthKey))
            {
                var token = GetToken(context);
                if (!string.IsNullOrEmpty(token))
                {
                    context.Request.Headers.Add(HeaderAuthKey, SuffixTokenValue + token);
                }
            }

            await _next.Invoke(context);
        }

        private static string GetToken(HttpContext context)
        {
            context.Request.Query.TryGetValue("access_token", out StringValues queryData);
            return queryData.FirstOrDefault();
        }
    }
}
