using System.ComponentModel.DataAnnotations;

namespace Auth_Back.DTOs.User
{
    public class UserUpdateDto
    {
        [Required]
        public string Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

    }
}
