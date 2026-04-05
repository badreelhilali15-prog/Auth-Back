using Auth_Back.Constants;
using Auth_Back.DTOs.Auth.LoginANDLogout;
using Auth_Back.DTOs.Auth.Register;
using Auth_Back.Mappers;
using Auth_Back.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
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
        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Userr not found"
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

            var token = GenerateJwtToken(user, roles);

        }
    }
}