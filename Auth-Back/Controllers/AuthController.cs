using Auth_Back.DTOs.Auth.Register;
using Auth_Back.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto dto)
        {
            var result = await _authService.RegisterAsync(dto);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
