using dotnet.Models;

namespace dotnet.Repositories.Interfaces
{
    /// <summary>
    /// Repository abstraction for User persistence.
    /// </summary>
    public interface IUserRepository
    {
        // PUBLIC_INTERFACE
        Task<User?> GetByUserNameAsync(string userName);

        // PUBLIC_INTERFACE
        Task<User?> GetByEmailAsync(string email);

        // PUBLIC_INTERFACE
        Task<User> AddAsync(User user);
    }
}
