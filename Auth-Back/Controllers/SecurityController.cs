using Auth_Back.Rsa;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;


namespace Auth_Back.Controllers
{
    [ApiController]
    [Route("api/security")]
    public class SecurityController : ControllerBase
    {
        [HttpGet("public-key")]
        public IActionResult GetPublicKey()
        {
            var rsa = RsaKeyProvider.GetPublicKey();

            var publicKey = Convert.ToBase64String(
                rsa.ExportSubjectPublicKeyInfo()
            );

            return Ok(publicKey);
        }
    }
}