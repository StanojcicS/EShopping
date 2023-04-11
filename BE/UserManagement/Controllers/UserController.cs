using Microsoft.AspNetCore.Mvc;
using UserManagement.Models.DataBase;
using UserManagement.Models.Incoming;
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
        public async Task<ActionResult> User(IncomingUser user)
        { 
            return Ok(await _userService.CreateUser(user));
        }
    }
}
