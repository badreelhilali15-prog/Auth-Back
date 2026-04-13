namespace Auth_Back.DTOs.User
{
    public class UserRoleDto
    {
        public string UserId { get; set; } = string.Empty;
        public IList<string> Roles { get; set; } = new List<string>();
    }
}
