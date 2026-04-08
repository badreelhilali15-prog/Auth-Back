using Auth_Back.DTOs;
using Auth_Back.DTOs.Auth.LoginANDLogout;
using Auth_Back.DTOs.Auth.Register;
using Auth_Back.DTOs.Auth.Token;
using Auth_Back.DTOs.Password;

namespace Auth_Back.Services
{
    public interface IAuthService
    {
        public  Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto dto);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
        Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);
        Task<bool> LogoutAsync(LogoutRequestDto request);


        Task<bool> ChangePasswordAsync(ChangePasswordDto dto);
        Task<ForgotPasswordResponseDto> ForgotPasswordAsync(ForgotPasswordRequestDto dto);
        Task<bool> ResetPasswordAsync(ResetPasswordDto dto);
        Task SendEmailAsync(string to);
        //Task<AuthResponseDto> LogoutAsync(LogoutRequestDto request);
    }
}
