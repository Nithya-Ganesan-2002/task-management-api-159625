using dotnet.Data;
using dotnet.Models;
using dotnet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace dotnet.Repositories
{
    /// <summary>
    /// EF Core implementation of IUserRepository.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;

        // PUBLIC_INTERFACE
        public UserRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // PUBLIC_INTERFACE
        public Task<User?> GetByUserNameAsync(string userName)
        {
            return _db.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        // PUBLIC_INTERFACE
        public Task<User?> GetByEmailAsync(string email)
        {
            return _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        // PUBLIC_INTERFACE
        public async Task<User> AddAsync(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }
    }
}
