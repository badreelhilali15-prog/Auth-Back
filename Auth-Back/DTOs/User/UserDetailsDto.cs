namespace Auth_Back.DTOs.User
{
    public class UserDetailsDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Tele { get; set; }
        public IList<string> Roles { get; set; }
        public bool TwoFactorEnabled { get; set; } // use for verifie the user is enable 2FA or not
        public DateTimeOffset? LockoutEnd { get; set; } // use for verifie the a count is blocked or not

    }
}
