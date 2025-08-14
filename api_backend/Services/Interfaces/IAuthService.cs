using dotnet.DTOs.Auth;

namespace dotnet.Services.Interfaces
{
    /// <summary>
    /// Authentication service for registering and logging in users.
    /// </summary>
    public interface IAuthService
    {
        // PUBLIC_INTERFACE
        Task<AuthResponse?> RegisterAsync(RegisterRequest request);

        // PUBLIC_INTERFACE
        Task<AuthResponse?> LoginAsync(LoginRequest request);
    }
}
