using Auth_Back.Constants;
using Auth_Back.DTOs.User;
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

        [HttpGet("all-user")]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _userService.GetAllUsers();
            return Ok(result);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, UserUpdateDto dto)
        {
            var result = await _userService.UpdateUser(id, dto);

            if (!result)
                return BadRequest("User update failed");

            return Ok("User updated successfully");
        }
        [HttpGet("roles/{id}")]
        public async Task<IActionResult> GetUserRoles(string id)
        {
            var result = await _userService.GetUserRoles(id);

            if (result == null)
                return NotFound("User not found");

            return Ok(result);
        }

        [HttpPut("roles")]
        public async Task<IActionResult> UpdateUserRoles(UserRoleDto dto)
        {
            var result = await _userService.UpdateUserRoles(dto);

            if (!result)
                return BadRequest("User role update failed");

            return Ok("User roles updated successfully");
        }
        [HttpPut("status")]
        public async Task<IActionResult> UpdateUserStatus(UserStatusDto dto)
        {
            var result = await _userService.UpdateUserStatus(dto);

            if (!result)
                return BadRequest("User status update failed");

            return Ok("User status updated successfully");
        }

        [HttpGet("simple/{id}")]
        public async Task<IActionResult> GetUserSimple(string id)
        {
            var result = await _userService.GetUserSimpleById(id);

            if (result == null)
                return NotFound("User not found");

            return Ok(result);
        }
    }
}
