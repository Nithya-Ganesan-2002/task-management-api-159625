using dotnet.DTOs.TaskDtos;

namespace dotnet.Services.Interfaces
{
    /// <summary>
    /// Task service encapsulating business logic for TaskItem entities.
    /// </summary>
    public interface ITaskService
    {
        // PUBLIC_INTERFACE
        Task<IEnumerable<TaskDto>> GetAllAsync();

        // PUBLIC_INTERFACE
        Task<TaskDto?> GetByIdAsync(Guid id);

        // PUBLIC_INTERFACE
        Task<TaskDto> CreateAsync(CreateTaskDto dto);

        // PUBLIC_INTERFACE
        Task<bool> UpdateAsync(Guid id, UpdateTaskDto dto);

        // PUBLIC_INTERFACE
        Task<bool> DeleteAsync(Guid id);
    }
}
