namespace Auth_Back.DTOs.Auth.LoginANDLogout
{
    public class AuthResponseDto
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public string Token { get; set; }

        public string RefreshToken { get; set; }

        public DateTime TokenExpiry { get; set; }

        public List<string> Errors { get; set; } = new();
    }
}
