using System.Runtime.InteropServices;
using BackendProjem.Authorization;
using BackendProjem.Domain;
using BackendProjem.Domain.Entities;
using BackendProjem.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendProjem.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestUserController : ControllerBase
    {
        private readonly ITestUserService _userService;
        public TestUserController(ITestUserService userService)
        {
            this._userService = userService;
        }

        [HttpPost("Insert")]
        public async Task<bool> Insert([FromBody] TestUser testUser)
        {
            return await this._userService.AddTestUser(testUser).ConfigureAwait(false);   
        }

        [HttpPut("Update")]
        public async Task<bool> Update([FromBody] TestUserUpdateRequestModel testUserUpdateRequestModel)
        {
            return await this._userService.UpdateUser(testUserUpdateRequestModel).ConfigureAwait(false);
        }


        [HttpDelete("Remove/{id}")]
        public async Task<bool> Remove(int id)
        {
            return await this._userService.RemoveUser(id).ConfigureAwait(false);
        }

        [HttpGet("GetUser/{id}")]
        public async Task<TestUser> GetUserById(int id)
        {
            return await this._userService.GetUserById(id).ConfigureAwait(false);
        }

        //[CustomAuthorization("Kübra")]
        // [Authorize]
        [HttpGet("Get")]
        public async Task<List<TestUser>> Get()
        {
            return await this._userService.GetUser().ConfigureAwait(false);

        }

    }
}
