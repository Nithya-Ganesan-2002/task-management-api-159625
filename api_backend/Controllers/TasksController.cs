using dotnet.DTOs.TaskDtos;
using dotnet.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet.Controllers
{
    /// <summary>
    /// Task management endpoints for TaskFlow API.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        // PUBLIC_INTERFACE
        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        /// <summary>
        /// List all tasks.
        /// </summary>
        /// <returns>Array of tasks.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TaskDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _taskService.GetAllAsync();
            return Ok(tasks);
        }

        /// <summary>
        /// Get a task by id.
        /// </summary>
        /// <param name="id">Task identifier (GUID).</param>
        /// <returns>Task details.</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var task = await _taskService.GetByIdAsync(id);
            return task is null ? NotFound() : Ok(task);
        }

        /// <summary>
        /// Create a new task.
        /// </summary>
        /// <param name="dto">Task creation payload.</param>
        /// <returns>The created task.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(TaskDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _taskService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Update an existing task.
        /// </summary>
        /// <param name="id">Task identifier (GUID).</param>
        /// <param name="dto">Task update payload.</param>
        /// <returns>No content on success.</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTaskDto dto)
        {
            var updated = await _taskService.UpdateAsync(id, dto);
            return updated ? NoContent() : NotFound();
        }

        /// <summary>
        /// Delete a task by id.
        /// </summary>
        /// <param name="id">Task identifier (GUID).</param>
        /// <returns>No content on success.</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleted = await _taskService.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}
