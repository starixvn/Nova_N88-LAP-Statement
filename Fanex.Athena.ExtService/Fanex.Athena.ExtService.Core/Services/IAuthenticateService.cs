using Fanex.Athena.ExtService.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fanex.Athena.ExtService.Core.Services
{
    public interface IAuthenticateService
    {
        Tuple<bool, Token> IsAuthenticated(TokenRequest request);
    }
}
