using Microsoft.AspNetCore.Identity;

namespace Auth_Back.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
