using Auth_Back.Constants;
using Auth_Back.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Auth_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles=Roles.Admin)]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet("{id}")]

        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _userService.GetUserDetailById(id);
            if (user == null)
            {
                return NotFound("User Not Found");
            }
            return Ok(user);
        }
    }
}
