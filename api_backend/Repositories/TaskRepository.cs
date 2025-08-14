using dotnet.Data;
using dotnet.Models;
using dotnet.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace dotnet.Repositories
{
    /// <summary>
    /// EF Core implementation of ITaskRepository.
    /// </summary>
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _db;

        // PUBLIC_INTERFACE
        public TaskRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        // PUBLIC_INTERFACE
        public async Task<IEnumerable<TaskItem>> GetAllAsync()
        {
            return await _db.Tasks
                .Include(t => t.AssignedToUser)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        // PUBLIC_INTERFACE
        public async Task<TaskItem?> GetByIdAsync(Guid id)
        {
            return await _db.Tasks
                .Include(t => t.AssignedToUser)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        // PUBLIC_INTERFACE
        public async Task<TaskItem> AddAsync(TaskItem task)
        {
            _db.Tasks.Add(task);
            await _db.SaveChangesAsync();
            return task;
        }

        // PUBLIC_INTERFACE
        public async Task UpdateAsync(TaskItem task)
        {
            task.UpdatedAt = DateTime.UtcNow;
            _db.Tasks.Update(task);
            await _db.SaveChangesAsync();
        }

        // PUBLIC_INTERFACE
        public async Task DeleteAsync(TaskItem task)
        {
            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync();
        }

        // PUBLIC_INTERFACE
        public Task<bool> ExistsAsync(Guid id)
        {
            return _db.Tasks.AnyAsync(t => t.Id == id);
        }
    }
}
