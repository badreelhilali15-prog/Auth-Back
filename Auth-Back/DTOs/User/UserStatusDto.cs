namespace Auth_Back.DTOs.User
{
    public class UserStatusDto
    {
        //cette classe perme de bloque ou debloe des user
        public string UserId { get; set; } = string.Empty;
        public bool IsLocked { get; set; }
    }
}
