using Fanex.Athena.ExtService.Core.Models;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Fanex.Athena.ExtService.Core.Services
{
    public class TokenAuthenticationService : IAuthenticateService
    {
        private readonly IUserManagementService _userManagementService;
        private readonly TokenManagement _tokenManagement;
        public TokenAuthenticationService(IUserManagementService service,
            IOptions<TokenManagement> tokenManagementAccessor)
        {
            _userManagementService = service;
            _tokenManagement = tokenManagementAccessor.Value;
        }
        public Tuple<bool, Token> IsAuthenticated(TokenRequest request)
        {
            var token = new Token();
            var siteName = request.Username;
            var realUserName = request.Username;

            if (request.Username.Contains("@"))
            {
                var temp = request.Username.Split('@');
                siteName = temp[0];
                realUserName = temp[1];
            }

            if (!_userManagementService.IsValidUser(realUserName, request.Password))
            {
                return Tuple.Create<bool, Token>(false, token);
            }

            var claim = new[]
            {
                new Claim(ClaimTypes.Name, siteName),
             };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var encryptCredenction = new EncryptingCredentials(key, JwtConstants.DirectKeyUseAlg,
                SecurityAlgorithms.Aes256CbcHmacSha512);
            var handler = new JwtSecurityTokenHandler();

            var jwtToken = handler.CreateJwtSecurityToken(_tokenManagement.Issuer,
                  _tokenManagement.Audience,
                  new ClaimsIdentity(claim),
                  DateTime.Now,
                  DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration),
                  null,
                  credentials,
                  encryptCredenction
                  );
            token.AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return Tuple.Create<bool, Token>(true, token);
        }
    }
}
