using Auth_Back.DTOs.User;
using Auth_Back.Models;

namespace Auth_Back.Mappers
{
    public class UserMapper
    {
        public static UserDetailsDto MapToUserDetailsDto (ApplicationUser user , IList<string>roles)
        {
            return new UserDetailsDto
            {
                Id = user.Id,
                UserName = user.FirstName+" "+ user.LastName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                //EmailConfirmed = user.EmailConfirmed,
               // PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                TwoFactorEnabled = user.TwoFactorEnabled,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEnd  = user.LockoutEnd,

                Roles = roles
            };
        }
    }
}
