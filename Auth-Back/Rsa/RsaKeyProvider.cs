using System.Security.Cryptography;

namespace Auth_Back.Rsa
{
    public static class RsaKeyProvider
    {
        public static readonly RSA _rsa;

        static RsaKeyProvider()
        {
            _rsa = RSAKey.GenrateKeyRSA();
        }

        public static RSA GetPrivateKey()
        {
            return _rsa;
        }
        public static RSA GetPublicKey()
        {
            var publicKey = RSA.Create();
            publicKey.ImportRSAPublicKey(_rsa.ExportRSAPublicKey(), out _);
            return publicKey;
        }
    }
}
