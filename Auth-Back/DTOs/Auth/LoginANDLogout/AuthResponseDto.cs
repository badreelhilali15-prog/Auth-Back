namespace Auth_Back.DTOs.Auth.LoginANDLogout
{
    public class AuthResponseDto
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public DateTime AccessTokenExpiry { get; set; }
        public string UserId { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; } = new();

        public List<string> Errors { get; set; } = new();
    }
}
