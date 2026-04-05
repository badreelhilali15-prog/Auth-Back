namespace Auth_Back.DTOs.Auth.Token
{
    public class RefreshTokenResponseDto
    {
        public bool IsSuccess { get; set; }
        public string NewAccessToken { get; set; }
        public string NewRefreshToken { get; set; }

        public DateTime AccessTokenExpiry { get; set; }

        public List<string> Errors { get; set; } = new();
    }
}
