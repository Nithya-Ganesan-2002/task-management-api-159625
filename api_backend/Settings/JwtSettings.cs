namespace dotnet.Settings
{
    /// <summary>
    /// Settings for JWT authentication (bound from configuration).
    /// Note: Use environment variables to configure these values in production.
    /// </summary>
    public class JwtSettings
    {
        // PUBLIC_INTERFACE
        public string Key { get; set; } = string.Empty;

        // PUBLIC_INTERFACE
        public string Issuer { get; set; } = string.Empty;

        // PUBLIC_INTERFACE
        public string Audience { get; set; } = string.Empty;

        // PUBLIC_INTERFACE
        public int ExpirationMinutes { get; set; } = 60;
    }
}
