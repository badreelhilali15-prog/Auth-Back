namespace Auth_Back.DTOs.User
{
    public class UserDetailsDto
    {

        public string Id { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }

        public bool EmailConfirmed { get; set; }
        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }// use for verifie the user is enable 2FA or not
        public bool LockoutEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }// use for verifie the a count is blocked or not

        public IList<string> Roles { get; set; } = new List<string>();
    
    }
}
