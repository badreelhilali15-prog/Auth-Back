using Auth_Back.DTOs;
using Auth_Back.DTOs.Auth.LoginANDLogout;
using Auth_Back.DTOs.Auth.Register;
using Auth_Back.DTOs.Auth.Token;
using Auth_Back.DTOs.Password;
using Auth_Back.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Auth_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<IdentityUser> _userManager;
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
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request);
            if (!result.IsSuccess)
            {
                return Unauthorized(result);// 4001
            }
            return Ok(result);//200
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenRequestDto request)
        {
            var result = await _authService.RefreshTokenAsync(request);
            if (result == null)
            {
                return Unauthorized();
            }
            return Ok(result);
        }
        [Authorize]
        [HttpPost("Secret")]
        public IActionResult Secret()
        {
            return Ok("This is authenticated users.");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutRequestDto request)
        {
            var result = await _authService.LogoutAsync(request);
            if (!result)
            {
                return NotFound("User not Found");
            }
            return Ok("logout success");
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            var result = await _authService.ChangePasswordAsync(dto);
            if (!result)
            {
                return BadRequest("Change password failed.");
            }
            return Ok("Password changed successfully.");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequestDto dto)
        {
            var result = await _authService.ForgotPasswordAsync(dto);
            return Ok(new
            {
                //if the email exists, we will send the reset password link
                message = "Check y email"
            });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto dto)
        {
            var result = await _authService.ResetPasswordAsync(dto);
            if (!result)
            {
                return BadRequest("Reset password failed.");
            }
            return Ok("Password reset successfully.");

            //var result = await _authService.ChangePasswordAsync(dto);
            //if (!result)
            //{
            //    return BadRequest("Change password failed.");
            //}
            //return Ok("Password changed successfully.");

        }
    }
}