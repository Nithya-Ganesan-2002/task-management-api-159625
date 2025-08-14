using dotnet.Models;

namespace dotnet.Repositories.Interfaces
{
    /// <summary>
    /// Repository abstraction for TaskItem persistence.
    /// </summary>
    public interface ITaskRepository
    {
        // PUBLIC_INTERFACE
        Task<IEnumerable<TaskItem>> GetAllAsync();

        // PUBLIC_INTERFACE
        Task<TaskItem?> GetByIdAsync(Guid id);

        // PUBLIC_INTERFACE
        Task<TaskItem> AddAsync(TaskItem task);

        // PUBLIC_INTERFACE
        Task UpdateAsync(TaskItem task);

        // PUBLIC_INTERFACE
        Task DeleteAsync(TaskItem task);

        // PUBLIC_INTERFACE
        Task<bool> ExistsAsync(Guid id);
    }
}
