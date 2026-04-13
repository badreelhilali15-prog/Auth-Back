using Auth_Back.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Auth_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IUserService _userService;

        public ProfileController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("profile")]
        
        public async Task<IActionResult> GetMyProfile()
        {
            string? userId = Request.Headers["X-User-Id"];

            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID missing in headers.");

            var result = await _userService.GetMyProfile(userId);

            if (result == null)
                return NotFound("User not found");

            return Ok(result);
        }
    }
}
