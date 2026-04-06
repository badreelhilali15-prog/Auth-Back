using Auth_Back.Constants;
using Auth_Back.DTOs.Auth.LoginANDLogout;
using Auth_Back.DTOs.Auth.Register;
using Auth_Back.DTOs.Auth.Token;
using Auth_Back.Mappers;
using Auth_Back.Models;
using Auth_Back.Provider;
using Auth_Back.Rsa;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
namespace Auth_Back.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        //  Register
        public async Task<RegisterResponseDto> RegisterAsync(RegisterRequestDto dto)
        {
            // basique validation
            if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            {
                return new RegisterResponseDto
                {
                    IsSuccess = false,
                    Message = "Email and password are required."
                };
            }

            // Normalisation email
            dto.Email = dto.Email.Trim().ToLower();

            //  verf mdp
            if (dto.Password != dto.ConfirmPassword)
            {
                return new RegisterResponseDto
                {
                    IsSuccess = false,
                    Message = "Passwords do not match."
                };
            }

            //  Check if email exist or npt
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);

            if (existingUser != null)
            {
                return new RegisterResponseDto
                {
                    IsSuccess = false,
                    Message = "Email is already in use."
                };
            }

            // appelle Mapper
            var user = AuthMapper.MapToApplicationUser(dto);

            //  Creat user and mdp has
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return new RegisterResponseDto
                {
                    IsSuccess = false,
                    Message = "Registration failed",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }

            //  Gere  securite rolles
            string roleToAssign = Roles.Client;

            if (!string.IsNullOrEmpty(dto.Role) && dto.Role == Roles.Freelancer)
            {
                roleToAssign = Roles.Freelancer;
            }

            // Admin jamais via register

            var roleResult = await _userManager.AddToRoleAsync(user, roleToAssign);

            if (!roleResult.Succeeded)
            {
                return new RegisterResponseDto
                {
                    IsSuccess = false,
                    Message = "User created but role assignment failed",
                    Errors = roleResult.Errors.Select(e => e.Description).ToList()
                };
            }

            // Succ register
            return new RegisterResponseDto
            {
                IsSuccess = true,
                Message = "User registered successfully"
            };
        }

        
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }

            var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!isValidPassword)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid password"
                };
            }

            var roles = await _userManager.GetRolesAsync(user);

            // 🔐 JWT
            var rsa = RsaKeyProvider.GetPrivateKey();
            var jwtGenerator = new JwtTokenGenerator(rsa);

            var accessToken = jwtGenerator.GenerateToken(
                user.Id,
                user.Email,
                roles.ToList()
            );

            // 🔁 Refresh Token
            var (refreshToken, expiry) = GenerateRefreshToken();
            var refreshTokenInfo = new RefreshTokenInfo
            {
                Token = refreshToken,
                ExpiryDate = DateTime.UtcNow.AddDays(7)
            };
             var json = JsonSerializer.Serialize(refreshTokenInfo);

            //var storedValue = $"{refreshToken}|{expiry:o}";

            await _userManager.SetAuthenticationTokenAsync(
                user,
                "MyApp",
                "RefreshToken",
                json
            );

            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "Login successful",
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                UserId = user.Id,
                Email = user.Email,
                Roles = roles.ToList(),
                AccessTokenExpiry = DateTime.UtcNow.AddMinutes(15)
            }; 
        }

        private (string token, DateTime expiry) GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
             var token = Convert.ToBase64String(randomBytes);
             var expiry = DateTime.UtcNow.AddDays(7); // 7 jours de vie4 de refresh token
            return (token, expiry);
        }
        public async Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return null;

            var stored = await _userManager.GetAuthenticationTokenAsync(
                user,
                "MyApp",
                "RefreshToken"
);

            if (stored == null)
                return null;

            // 🔥 séparation token + date
            var parts = stored.Split('|');

            //var savedToken = parts[0];
            //var expiry = DateTime.Parse(parts[1]);

            //// 🔐 vérifier token
            //if (savedToken != request.RefreshToken)
            //    return null;

            //// ⏳ vérifier expiration
            //if (expiry < DateTime.UtcNow)
            //    return null;

            var savedTokenJson = await _userManager.GetAuthenticationTokenAsync(
                user,
                "MyApp",
                "RefreshToken"
);

            var tokenInfo = JsonSerializer.Deserialize<RefreshTokenInfo>(savedTokenJson);

            if (tokenInfo.Token != request.RefreshToken)
                return null;

            if (tokenInfo.ExpiryDate < DateTime.UtcNow)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            var rsa = RsaKeyProvider.GetPrivateKey();
            var jwtGenerator = new JwtTokenGenerator(rsa);

            var newAccessToken = jwtGenerator.GenerateToken(
                user.Id,
                user.Email,
                roles.ToList()
            );

            var (newRefreshToken, newExpiry) = GenerateRefreshToken();

            var storedValue = $"{newRefreshToken}|{newExpiry:o}";

            await _userManager.SetAuthenticationTokenAsync(
                user,
                "MyApp",
                "RefreshToken",
                storedValue
            );
            return new RefreshTokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
        public async Task<bool> LogoutAsync(LogoutRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return false;
            await _userManager.RemoveAuthenticationTokenAsync(
                user,
                "MyApp",
                "RefreshToken"
            );
            return true;
        }
        // Login
        //public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        //{
        //    var user = await _userManager.FindByEmailAsync(request.Email);

        //    if (user == null)
        //    {
        //        return new AuthResponseDto
        //        {
        //            IsSuccess = false,
        //            Message = "Userr not found"
        //        };
        //    }

        //    var isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);

        //    if (!isValidPassword)
        //    {
        //        return new AuthResponseDto
        //        {
        //            IsSuccess = false,
        //            Message = "Invalid password"
        //        };
        //    }

        //    var roles = await _userManager.GetRolesAsync(user);

        //    var rsa = RsaKeyProvider.GetPrivateKey();

        //    var jwtGenerator = new JwtTokenGenerator(rsa);

        //    var token = jwtGenerator.GenerateToken(
        //        user.Id,
        //        user.Email,
        //        roles.ToList()
        //    );

        //    return new AuthResponseDto
        //    {
        //        IsSuccess = true,
        //        AccessToken = token,
        //        UserId = user.Id,
        //        Email = user.Email,
        //        Roles = roles.ToList(),
        //        AccessTokenExpiry = DateTime.UtcNow.AddHours(2)
        //    };

        //}
    }
}
