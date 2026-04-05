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
            var refreshToken = GenerateRefreshToken();

            // 💾 Store Refresh Token in Identity
            await _userManager.SetAuthenticationTokenAsync(
                user,
                "MyApp",
                "RefreshToken",
                refreshToken
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

        private string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }
        public async Task<RefreshTokenResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return null;

            var savedToken = await _userManager.GetAuthenticationTokenAsync(
                user,
                "MyApp",
                "RefreshToken"
            );

            // 🔐 Vérification
            if (savedToken != request.RefreshToken)
                return null;

            var roles = await _userManager.GetRolesAsync(user);

            var rsa = RsaKeyProvider.GetPrivateKey();
            var jwtGenerator = new JwtTokenGenerator(rsa);

            var newAccessToken = jwtGenerator.GenerateToken(
                user.Id,
                user.Email,
                roles.ToList()
            );

            // 🔥 rotation refresh token
            var newRefreshToken = GenerateRefreshToken();

            await _userManager.SetAuthenticationTokenAsync(
                user,
                "MyApp",
                "RefreshToken",
                newRefreshToken
            );

            return new RefreshTokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }
    }
}
