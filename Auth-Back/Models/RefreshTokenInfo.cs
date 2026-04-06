namespace Auth_Back.Models
{
    public class RefreshTokenInfo
    {
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
