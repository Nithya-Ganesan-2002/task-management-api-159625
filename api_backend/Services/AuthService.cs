using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using dotnet.DTOs.Auth;
using dotnet.Models;
using dotnet.Repositories.Interfaces;
using dotnet.Services.Interfaces;
using dotnet.Services.Security;
using dotnet.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace dotnet.Services
{
    /// <summary>
    /// Authentication service for registering and logging in users.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly JwtSettings _jwtSettings;

        // PUBLIC_INTERFACE
        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher, IOptions<JwtSettings> jwtOptions)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
            _jwtSettings = jwtOptions.Value ?? new JwtSettings();
        }

        // PUBLIC_INTERFACE
        public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
        {
            var existingByUser = await _userRepository.GetByUserNameAsync(request.UserName);
            if (existingByUser != null) return null;

            var existingByEmail = await _userRepository.GetByEmailAsync(request.Email);
            if (existingByEmail != null) return null;

            var (hash, salt) = _passwordHasher.HashPassword(request.Password);
            var user = new User
            {
                UserName = request.UserName,
                Email = request.Email,
                PasswordHash = hash,
                PasswordSalt = salt
            };
            user = await _userRepository.AddAsync(user);

            var token = GenerateJwtToken(user, out var expiresAt);
            return new AuthResponse
            {
                Token = token,
                ExpiresAt = expiresAt,
                UserName = user.UserName,
                Email = user.Email
            };
        }

        // PUBLIC_INTERFACE
        public async Task<AuthResponse?> LoginAsync(LoginRequest request)
        {
            User? user = request.UserNameOrEmail.Contains("@")
                ? await _userRepository.GetByEmailAsync(request.UserNameOrEmail)
                : await _userRepository.GetByUserNameAsync(request.UserNameOrEmail);

            if (user is null) return null;
            if (!_passwordHasher.Verify(request.Password, user.PasswordHash, user.PasswordSalt)) return null;

            var token = GenerateJwtToken(user, out var expiresAt);
            return new AuthResponse
            {
                Token = token,
                ExpiresAt = expiresAt,
                UserName = user.UserName,
                Email = user.Email
            };
        }

        private string GenerateJwtToken(User user, out DateTime expiresAt)
        {
            // Fallback to environment variables if not set in options (supporting both Jwt:Key and JWT__Key)
            var key = _jwtSettings.Key
                      ?? Environment.GetEnvironmentVariable("Jwt__Key")
                      ?? Environment.GetEnvironmentVariable("JWT__Key")
                      ?? "temporary-development-key-change-me";
            var issuer = _jwtSettings.Issuer
                        ?? Environment.GetEnvironmentVariable("Jwt__Issuer")
                        ?? Environment.GetEnvironmentVariable("JWT__Issuer")
                        ?? "taskflow.local";
            var audience = _jwtSettings.Audience
                          ?? Environment.GetEnvironmentVariable("Jwt__Audience")
                          ?? Environment.GetEnvironmentVariable("JWT__Audience")
                          ?? "taskflow.clients";
            var expiresMinutes = _jwtSettings.ExpirationMinutes > 0 ? _jwtSettings.ExpirationMinutes : 60;

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new(JwtRegisteredClaimNames.Email, user.Email)
            };

            expiresAt = DateTime.UtcNow.AddMinutes(expiresMinutes);
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
