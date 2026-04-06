using Auth_Back.DTOs.Auth.LoginANDLogout;
using Auth_Back.DTOs.Auth.Register;
using Auth_Back.DTOs.Auth.Token;

namespace Auth_Back.Services
{
    public interface IAuthService
    {
        public  Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto dto);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
        Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request);
        Task<bool> LogoutAsync(LogoutRequestDto request);

        //Task<AuthResponseDto> LogoutAsync(LogoutRequestDto request);
    }
}
