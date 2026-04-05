using System.ComponentModel.DataAnnotations;

namespace Auth_Back.DTOs.Auth.Register
{
    public class RegisterRequestDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Passwords is not match.")]
        public string ConfirmPassword { get; set; }

        public string Role { get; set; } 
    }
}
