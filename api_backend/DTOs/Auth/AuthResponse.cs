namespace dotnet.DTOs.Auth
{
    /// <summary>
    /// Authentication response with JWT token.
    /// </summary>
    public class AuthResponse
    {
        // PUBLIC_INTERFACE
        public string Token { get; set; } = string.Empty;

        // PUBLIC_INTERFACE
        public DateTime ExpiresAt { get; set; }

        // PUBLIC_INTERFACE
        public string UserName { get; set; } = string.Empty;

        // PUBLIC_INTERFACE
        public string Email { get; set; } = string.Empty;
    }
}
