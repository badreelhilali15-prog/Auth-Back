using Auth_Back.Constants;
using Auth_Back.DTOs.User;
using Auth_Back.Mappers;
using Auth_Back.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<UserListDto>> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var result = new List<UserListDto>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                result.Add(UserMapper.MapToUserListDto(user, roles));
            }

            return result;
        }

        public async Task<bool> UpdateUser(string userId, UserUpdateDto dto)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return false;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return false;

            user.FirstName = dto.FirstName?.Trim() ?? user.FirstName;
            user.LastName = dto.LastName?.Trim() ?? user.LastName;
            user.Email = dto.Email?.Trim() ?? user.Email;
            user.PhoneNumber = dto.PhoneNumber;

            if (dto.TwoFactorEnabled.HasValue)
            {
                user.TwoFactorEnabled = dto.TwoFactorEnabled.Value;
            }

            var result = await _userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<bool> UpdateUserRoles(UserRoleDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId))
                return false;

            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
                return false;

            var currentRoles = await _userManager.GetRolesAsync(user);

            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
                return false;

            var allowedRoles = new List<string>
            {
                Roles.Admin,
                Roles.Client,
                Roles.Freelancer
            };

            var validRoles = dto.Roles
                .Where(r => allowedRoles.Contains(r))
                .Distinct()
                .ToList();

            if (validRoles.Count == 0)
                return false;

            var addResult = await _userManager.AddToRolesAsync(user, validRoles);

            return addResult.Succeeded;
        }
        public async Task<UserRoleDto?> GetUserRoles(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return null;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            return new UserRoleDto
            {
                UserId = user.Id,
                Roles = roles
            };
        }
        public async Task<bool> UpdateUserStatus(UserStatusDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.UserId))
                return false;

            var user = await _userManager.FindByIdAsync(dto.UserId);

            if (user == null)
                return false;

            if (dto.IsLocked)
            {
                user.LockoutEnabled = true;
                user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);
            }
            else
            {
                user.LockoutEnd = null;
            }

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public async Task<UserProfileDto?> GetMyProfile(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return null;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            return UserMapper.MapToUserProfileDto(user, roles);
        }

        public async Task<UserDto?> GetUserSimpleById(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                return null;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            return UserMapper.MapToUserDto(user, roles);
        }
    }
}
