using Microsoft.AspNetCore.Http;
using System.Net;

namespace Fanex.Athena.ExtService.Core.Services
{
    public interface IClientService
    {
        IPAddress RealClientIP { get; }

        HttpContext Context { get; }
    }
}