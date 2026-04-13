using System.ComponentModel.DataAnnotations;

namespace Auth_Back.DTOs.User
{
    public class UserUpdateDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? TwoFactorEnabled { get; set; }

    }
}
