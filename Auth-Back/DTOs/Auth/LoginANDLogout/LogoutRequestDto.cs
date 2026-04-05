namespace Auth_Back.DTOs.Auth.LoginANDLogout
{
    public class LogoutRequestDto
    {
        public string UserId { get; set; }

        public string RefreshToken { get; set; }
    }
}
