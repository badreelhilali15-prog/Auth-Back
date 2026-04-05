namespace Auth_Back.DTOs.User
{
    public class UserListDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }//first name and last name
        public string UserEmail { get; set; }
        public List<string> Roles { get; set; }= new List<string>();

        public bool IsActive { get; set; }// Property calc or personalis
        
        //Je pense que en doit ajoute une paginaton ici

    }
}
