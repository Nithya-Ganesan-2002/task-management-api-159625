using System.ComponentModel.DataAnnotations;

namespace dotnet.DTOs.Auth
{
    /// <summary>
    /// Register request payload.
    /// </summary>
    public class RegisterRequest
    {
        // PUBLIC_INTERFACE
        [Required, MaxLength(64)]
        public string UserName { get; set; } = string.Empty;

        // PUBLIC_INTERFACE
        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        // PUBLIC_INTERFACE
        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;
    }
}
