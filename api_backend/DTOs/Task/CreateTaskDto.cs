using System.ComponentModel.DataAnnotations;
using dotnet.Models;

namespace dotnet.DTOs.TaskDtos
{
    /// <summary>
    /// Request payload for creating a task.
    /// </summary>
    public class CreateTaskDto
    {
        // PUBLIC_INTERFACE
        [Required, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        // PUBLIC_INTERFACE
        public string? Description { get; set; }

        // PUBLIC_INTERFACE
        public dotnet.Models.TaskStatus Status { get; set; } = dotnet.Models.TaskStatus.Todo;

        // PUBLIC_INTERFACE
        public DateTime? DueDate { get; set; }

        // PUBLIC_INTERFACE
        public Guid? AssignedToUserId { get; set; }
    }
}
