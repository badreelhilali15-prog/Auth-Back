namespace Auth_Back.DTOs.Auth.Token
{
    public class RefreshTokenRequestDto
    {
        //public string AccessToken { get; set; }
        //public string RefreshToken { get; set; }
        //public string UserId { get; set; }

        public string Email { get; set; }

        public string RefreshToken { get; set; }
    }
}
