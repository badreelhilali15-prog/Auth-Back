using Auth_Back.DTOs.User;

namespace Auth_Back.Services
{
    public interface IUserService
    {
        Task<UserDetailsDto?> GetUserDetailById(String userId);
        Task<List<UserListDto>> GetAllUsers();
    }
}
