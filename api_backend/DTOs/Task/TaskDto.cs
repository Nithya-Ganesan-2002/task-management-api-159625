using dotnet.Models;
using DomainTaskStatus = dotnet.Models.TaskStatus;

namespace dotnet.DTOs.TaskDtos
{
    /// <summary>
    /// Response payload representing a task.
    /// </summary>
    public class TaskDto
    {
        // PUBLIC_INTERFACE
        public Guid Id { get; set; }

        // PUBLIC_INTERFACE
        public string Title { get; set; } = string.Empty;

        // PUBLIC_INTERFACE
        public string? Description { get; set; }

        // PUBLIC_INTERFACE
        public DomainTaskStatus Status { get; set; }

        // PUBLIC_INTERFACE
        public DateTime? DueDate { get; set; }

        // PUBLIC_INTERFACE
        public Guid? AssignedToUserId { get; set; }

        // PUBLIC_INTERFACE
        public DateTime CreatedAt { get; set; }

        // PUBLIC_INTERFACE
        public DateTime UpdatedAt { get; set; }
    }
}
