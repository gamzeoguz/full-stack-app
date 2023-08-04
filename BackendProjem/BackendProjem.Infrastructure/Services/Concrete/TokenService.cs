using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.IdentityModel.Tokens;

namespace BackendProjem.Infrastructure
{

    public class TokenService : ITokenService
    {
        private readonly IdentityServerTools _identityTool;
        private readonly ITestUserService _userService;
        public TokenService(ITestUserService userService, IdentityServerTools identityTool)
        {
            _userService = userService;
            _identityTool = identityTool;

        }
        public async Task<string> GetToken(string email, string password)
        {
            var result = await _userService.CheckUserByEmail(email, password).ConfigureAwait(false);
            if (!result)
            {
                return string.Empty;
            }
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim("ClientType", "Gözde"));

            var accessToken = await _identityTool.IssueClientJwtAsync("Company_Client_Id",
                                3600,
                                additionalClaims: claims,
                                audiences: new[] { "Company_Scope_Api_1" },
                                scopes: new[] { "Company_Scope_Api_1" });

            return accessToken;

            
        }
    }
}
