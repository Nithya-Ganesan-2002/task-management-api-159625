using System.ComponentModel.DataAnnotations;

namespace dotnet.DTOs.Auth
{
    /// <summary>
    /// Login request payload.
    /// </summary>
    public class LoginRequest
    {
        // PUBLIC_INTERFACE
        [Required]
        public string UserNameOrEmail { get; set; } = string.Empty;

        // PUBLIC_INTERFACE
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
