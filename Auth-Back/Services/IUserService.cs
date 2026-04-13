using Auth_Back.DTOs.User;

namespace Auth_Back.Services
{
    public interface IUserService
    {
        Task<UserDetailsDto?> GetUserDetailById(String userId);
        Task<List<UserListDto>> GetAllUsers();
        Task<bool> UpdateUser(string userId, UserUpdateDto dto);
        Task<bool> UpdateUserRoles(UserRoleDto dto);
        Task<UserRoleDto?> GetUserRoles(string userId);
        Task<bool> UpdateUserStatus(UserStatusDto dto);
        Task<UserProfileDto?> GetMyProfile(string userId);
        Task<UserDto?> GetUserSimpleById(string userId);
    }
}
