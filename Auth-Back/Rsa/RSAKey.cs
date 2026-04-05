using System.Security.Cryptography;
namespace Auth_Back.Rsa
{
    public class RSAKey
    {
        public static RSA GenrateKeyRSA()
        {
            var rsa = RSA.Create();
            rsa.KeySize = 2048; // Taille de clé 
            return rsa;
        }
        public static string ExportPrivateKey(RSA rsa)
        {
            return Convert.ToBase64String(rsa.ExportRSAPrivateKey());
        }

        public static string ExportPublicKey(RSA rsa)
        {
            return Convert.ToBase64String(rsa.ExportRSAPublicKey());
        }

        public static RSA ImportPrivateKey(string privateKey)
        {
            var rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
            return rsa;
        }
        public static RSA ImportPublicKey(string publicKey)
        {
            var rsa = RSA.Create();
            rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
            return rsa;
        }


        }
}
