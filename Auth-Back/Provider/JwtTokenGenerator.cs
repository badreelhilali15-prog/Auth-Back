using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Auth_Back.Provider
{
    // generat token foor the cle privet
    public class JwtTokenGenerator
    {
        private readonly RSA _privateRsa;

        public JwtTokenGenerator(RSA privateRsa)
        {
            _privateRsa = privateRsa;
        }

        public string GenerateToken(string userId, string email, List<string> roles)
        {
            var credentials = new SigningCredentials(
                new RsaSecurityKey(_privateRsa),
                SecurityAlgorithms.RsaSha256
            );

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Email, email)
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: "MyApp",
                audience: "MyClient",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
