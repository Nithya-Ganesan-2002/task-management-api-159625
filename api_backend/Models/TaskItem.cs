using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace dotnet.Models
{
    /// <summary>
    /// Represents a task item within TaskFlow.
    /// </summary>
    public class TaskItem
    {
        // PUBLIC_INTERFACE
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // PUBLIC_INTERFACE
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        // PUBLIC_INTERFACE
        public string? Description { get; set; }

        // PUBLIC_INTERFACE
        public TaskStatus Status { get; set; } = TaskStatus.Todo;

        // PUBLIC_INTERFACE
        public DateTime? DueDate { get; set; }

        // PUBLIC_INTERFACE
        public Guid? AssignedToUserId { get; set; }

        [ForeignKey(nameof(AssignedToUserId))]
        public User? AssignedToUser { get; set; }

        // PUBLIC_INTERFACE
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // PUBLIC_INTERFACE
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    /// <summary>
    /// Task status values.
    /// </summary>
    public enum TaskStatus
    {
        Todo = 0,
        InProgress = 1,
        Done = 2,
        Blocked = 3
    }
}
