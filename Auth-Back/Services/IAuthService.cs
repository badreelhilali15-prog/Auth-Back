using Auth_Back.DTOs.Auth.Register;

namespace Auth_Back.Services
{
    public interface IAuthService
    {
        public  Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto dto);
    }
}
