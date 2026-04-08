namespace Auth_Back.DTOs.User
{
    public class UserListDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }//first name and last name
        public string Email { get; set; }
        public IList<string> Roles { get; set; }= new List<string>();

        public bool IsActive { get; set; }// Property calc or personalis

        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        //Je pense que en doit ajoute une paginaton ici

    }
}
