using BackendProjem.Domain.Model;
using BackendProjem.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BackendProjem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController: ControllerBase
    {
        private readonly ITokenService _tokenService;
        public TokenController(ITokenService tokenService)
        {
            this._tokenService = tokenService;
        }

        [HttpPost("Get")]
        public async Task<string> GetToken([FromBody] TokenRequestModel request)
        {
            var result = await this._tokenService.GetToken(request.Email, request.Password).ConfigureAwait(false);
            return result;
        }
    }
}
