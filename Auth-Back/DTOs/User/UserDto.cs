using Org.BouncyCastle.Bcpg.Sig;
using System.Globalization;

namespace Auth_Back.DTOs.User
{
    public class UserDto
    {
        // pour un affichage simple de user pas de detiale 
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public IList<string> Roles { get; set; } = new List<string>();
        public bool IsActive { get; set; }// en save si lke compte et active ou non
        public bool TwoFactorEnabled { get; set; }


    }
}
