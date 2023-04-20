using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Common.Models.Incoming; 
using UserManagement.Services.Contracts;

namespace UserManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController :ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<ActionResult> User([FromForm] RegisterUser user)
        { 
            return Ok(await _userService.CreateUser(user));
        }

        [HttpPost]
        [Route("/login")]
        public async Task<ActionResult> Login(LoginUser user)
        {

            return Ok(await _userService.Login(user.Username, user.Password));
        }

        [HttpPatch]
        [Route("/users/{username}/verify")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Verify(string username)
        {
            return Ok(await _userService.Verify(username));
        }
    }
}
