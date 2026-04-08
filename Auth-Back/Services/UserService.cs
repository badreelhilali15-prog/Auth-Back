using Auth_Back.DTOs.User;
using Auth_Back.Mappers;
using Auth_Back.Models;
using Microsoft.AspNetCore.Identity;

namespace Auth_Back.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            
        }

        public async Task <UserDetailsDto> GetUserDetailById(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return null;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            return UserMapper.MapToUserDetailsDto(user, roles);

        }
    }
}
