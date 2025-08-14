namespace dotnet.Services.Security
{
    /// <summary>
    /// Abstraction for password hashing and verification.
    /// </summary>
    public interface IPasswordHasher
    {
        // PUBLIC_INTERFACE
        (string hash, string salt) HashPassword(string password);

        // PUBLIC_INTERFACE
        bool Verify(string password, string hash, string salt);
    }
}
