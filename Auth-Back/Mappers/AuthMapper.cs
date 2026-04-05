using Auth_Back.DTOs.Auth.Register;
using Auth_Back.Models;
using System.ComponentModel.DataAnnotations;

namespace Auth_Back.Mappers
{
    public class AuthMapper
    {
        public static ApplicationUser MapToApplicationUser(RegisterRequestDto dto)
        {
            return new ApplicationUser
            {
                Email = dto.Email,
                UserName = dto.FirstName + dto.LastName, 
                FirstName = dto.FirstName,
                LastName = dto.LastName,
               

            };
        }
    }
}
