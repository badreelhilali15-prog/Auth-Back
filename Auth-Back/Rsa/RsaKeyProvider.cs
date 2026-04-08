using System.Security.Cryptography;

namespace Auth_Back.Rsa
{
    public static class RsaKeyProvider
    {
        public static readonly RSA _privateRsa;

        static RsaKeyProvider()
        {
            _privateRsa = LoadPrivateKey();
        }

        public static RSA LoadPrivateKey()
        {
            var pem= File.ReadAllText("Keys/private_key.pem");
            var rsa = RSA.Create();
            rsa.ImportFromPem(pem.ToCharArray());
            return rsa;
        }
        public static RSA GetPrivateKey()
        {
            return _privateRsa;
        }
        public static RSA GetPublicKey()
        {
            var pem =File .ReadAllText("Keys/public_key.pem");
            var rsa = RSA.Create();
            rsa.ImportFromPem(pem.ToCharArray());
            return rsa;
        }
    }
}

