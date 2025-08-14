using System.ComponentModel.DataAnnotations;

namespace dotnet.Models
{
    /// <summary>
    /// Represents an application user.
    /// </summary>
    public class User
    {
        // PUBLIC_INTERFACE
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // PUBLIC_INTERFACE
        [Required, MaxLength(64)]
        public string UserName { get; set; } = string.Empty;

        // PUBLIC_INTERFACE
        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        // PUBLIC_INTERFACE
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // PUBLIC_INTERFACE
        [Required]
        public string PasswordSalt { get; set; } = string.Empty;

        // PUBLIC_INTERFACE
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
