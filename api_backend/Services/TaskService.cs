using dotnet.DTOs.TaskDtos;
using dotnet.Models;
using dotnet.Repositories.Interfaces;

namespace dotnet.Services
{
    /// <summary>
    /// Business logic for TaskItem.
    /// </summary>
    public class TaskService : dotnet.Services.Interfaces.ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        // PUBLIC_INTERFACE
        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        // PUBLIC_INTERFACE
        public async Task<IEnumerable<TaskDto>> GetAllAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return tasks.Select(MapToDto);
        }

        // PUBLIC_INTERFACE
        public async Task<TaskDto?> GetByIdAsync(Guid id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            return task is null ? null : MapToDto(task);
        }

        // PUBLIC_INTERFACE
        public async Task<TaskDto> CreateAsync(CreateTaskDto dto)
        {
            var entity = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                DueDate = dto.DueDate,
                AssignedToUserId = dto.AssignedToUserId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            entity = await _taskRepository.AddAsync(entity);
            return MapToDto(entity);
        }

        // PUBLIC_INTERFACE
        public async Task<bool> UpdateAsync(Guid id, UpdateTaskDto dto)
        {
            var existing = await _taskRepository.GetByIdAsync(id);
            if (existing is null) return false;

            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.Status = dto.Status;
            existing.DueDate = dto.DueDate;
            existing.AssignedToUserId = dto.AssignedToUserId;
            existing.UpdatedAt = DateTime.UtcNow;

            await _taskRepository.UpdateAsync(existing);
            return true;
        }

        // PUBLIC_INTERFACE
        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _taskRepository.GetByIdAsync(id);
            if (existing is null) return false;

            await _taskRepository.DeleteAsync(existing);
            return true;
        }

            private static TaskDto MapToDto(TaskItem t)
            {
                return new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    Status = t.Status,
                    DueDate = t.DueDate,
                    AssignedToUserId = t.AssignedToUserId,
                    CreatedAt = t.CreatedAt,
                    UpdatedAt = t.UpdatedAt
                };
            }
    }
}
