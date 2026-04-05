using System.Globalization;

namespace Auth_Back.DTOs.User
{
    public class UserDto
    { 
        //pour affiche les info user 
        //retourner les donnees apre login 
        //affiche proile user 
        //envoyer liste user 
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }


   
    }
}
