using Auth_Back.DTOs.User;
using Auth_Back.Models;

namespace Auth_Back.Mappers
{
    public class UserMapper
    {
        public static UserDetailsDto MapToUserDetailsDto(ApplicationUser user, IList<string> roles)
        {
            return new UserDetailsDto
            {
                Id = user.Id,
                UserName = user.FirstName + " " + user.LastName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                //EmailConfirmed = user.EmailConfirmed,
                // PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEnd = user.LockoutEnd,

                Roles = roles
            };
        }

        public static UserListDto MapToUserListDto(ApplicationUser user, IList<string> roles)
        {
            bool isActive = user.LockoutEnd == null || user.LockoutEnd <= DateTimeOffset.UtcNow;

            return new UserListDto
            {
                Id = user.Id,
                UserName = $"{user.FirstName} {user.LastName}".Trim(),
                Email = user.Email ?? string.Empty,
                Roles = roles,
                IsActive = isActive,
                TwoFactorEnabled = user.TwoFactorEnabled,
                LockoutEnd = user.LockoutEnd,
                
            };
        }

        public static UserDto MapToUserDto(ApplicationUser user, IList<string> roles)
        {
            bool isActive = user.LockoutEnd == null || user.LockoutEnd <= DateTimeOffset.UtcNow;

            return new UserDto
            {
                Id = user.Id,
                FullName = $"{user.FirstName} {user.LastName}".Trim(),
                Email = user.Email ?? string.Empty,
                Roles = roles,
                IsActive = isActive,
                TwoFactorEnabled = user.TwoFactorEnabled
            };
        }
        public static UserProfileDto MapToUserProfileDto(ApplicationUser user, IList<string> roles)
        {
            return new UserProfileDto
            {
                Id = user.Id,
                UserName = user.UserName ?? string.Empty,
                FullName = $"{user.FirstName} {user.LastName}".Trim(),
                Email = user.Email ?? string.Empty,
                PhoneNumber = user.PhoneNumber,
                EmailConfirmed = user.EmailConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled,
                Roles = roles
            };
        }
    }
}
