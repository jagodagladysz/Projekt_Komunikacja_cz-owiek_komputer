using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlannerProjekt.Dtos;
using PlannerProjekt.Entities;
using PlannerProjekt.Extentions;
using PlannerProjekt.Services;
using System.Security.Claims;

namespace PlannerProjekt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "UserPolicy")]
    public class PlannerController : ControllerBase
    {
        private readonly PlannerService _plannerService;
        private readonly SetTimeService _setTimeService;

        public PlannerController(PlannerService plannerService, SetTimeService setTimeService)
        {
            _plannerService = plannerService;
            _setTimeService = setTimeService;
        }

        // authorization
        private string GetUsernameFromToken()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
            {
                throw new UnauthorizedAccessException("Invalid token.");
            }
            return username;
        }

        [HttpGet("get-tasks")]
        public async Task<IActionResult> GetTasksByUser()
        {
            var username = GetUsernameFromToken();
            var user = await _plannerService.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            var tasks = await _setTimeService.GetTasksByUserIdAsync(user.Id);

            if (tasks == null || tasks.Count == 0)
            {
                return NotFound();
            }

            return Ok(tasks);
        }
        [HttpGet("task-with-subtasks/{taskId}")]
        public async Task<IActionResult> GetTaskWithSubTasks(int taskId)
        {
            var username = GetUsernameFromToken();
            var user = await _plannerService.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            var taskWithSubTasks = await _plannerService.GetTaskWithSubTasksAsync(taskId, user);

            if (taskWithSubTasks == null)
            {
                return NotFound("Task not found.");
            }

            return Ok(taskWithSubTasks);
        }

        [HttpPost("add-task")]
        public async Task<IActionResult> AddTask([FromBody] TaskDto taskDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var username = GetUsernameFromToken();
            var user = await _plannerService.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            var createdTask = await _plannerService.AddTaskAsync(taskDto, user.Id);
            var createdTaskDto = createdTask.ToDto(); 

            return Ok(createdTaskDto);
        }

        [HttpPut("update-task/{taskId}")]
        public async Task<IActionResult> UpdateTask(int taskId, [FromBody] TaskDto taskDto)
        {
            var username = GetUsernameFromToken();
            var user = await _plannerService.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            var updatedTask = await _plannerService.UpdateTaskAsync(taskId, taskDto, user);

            if (updatedTask == null)
            {
                return NotFound("Task not found.");
            }

            var updatedTaskDto = updatedTask.ToDto();
            return Ok(updatedTaskDto);
        }

        [HttpDelete("delete-task/{taskId}")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var username = GetUsernameFromToken();
            var user = await _plannerService.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            var deleted = await _plannerService.DeleteTaskAsync(taskId, user);

            if (!deleted)
            {
                return NotFound("Task not found.");
            }

            return Ok("Task deleted successfully.");
        }

        [HttpPut("mark-task-completed/{taskId}")]
        public async Task<IActionResult> MarkTaskAsCompleted(int taskId)
        {
            var username = GetUsernameFromToken();
            var user = await _plannerService.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            var updatedTaskDto = await _plannerService.MarkTaskAsCompletedAsync(taskId, user);

            if (updatedTaskDto == null)
            {
                return NotFound("Task not found.");
            }

            return Ok(updatedTaskDto);
        }

        [HttpGet("completed-tasks")]
        public async Task<IActionResult> GetCompletedTasks()
        {
            var username = GetUsernameFromToken();
            var user = await _plannerService.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            var tasks = await _plannerService.GetCompletedTasksAsync(user);
            return Ok(tasks);
        }

        [HttpGet("incomplete-tasks")]
        public async Task<IActionResult> GetIncompleteTasks()
        {
            var username = GetUsernameFromToken();
            var user = await _plannerService.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            var tasks = await _plannerService.GetIncompleteTasksAsync(user);
            return Ok(tasks);
        }

        [HttpPost("{taskId}/add-subtasks")]
        public async Task<IActionResult> AddSubTask(int taskId, [FromBody] SubTaskDto subTaskDto)
        {
            var username = GetUsernameFromToken();

            try
            {
                var createdSubTask = await _plannerService.AddSubTaskAsync(subTaskDto, taskId, username);
                var createdSubTaskDto = createdSubTask.ToDto(); 
                return Ok(createdSubTaskDto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{taskId}/get-subtasks")]
        public async Task<IActionResult> GetSubTasks(int taskId)
        {
            var username = GetUsernameFromToken();

            try
            {
                var subTasks = await _plannerService.GetSubTasksForTaskAsync(taskId, username);
                var subTasksDto = subTasks.Select(st => st.ToDto()).ToList();
                return Ok(subTasksDto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpDelete("delete-subtask/{subTaskId}")]
        public async Task<IActionResult> DeleteSubTask(int subTaskId)
        {
            var username = GetUsernameFromToken();
            var user = await _plannerService.GetUserByUsernameAsync(username);

            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            var result = await _plannerService.DeleteSubTaskAsync(subTaskId, user);

            if (!result)
            {
                return NotFound("SubTask not found or could not be deleted.");
            }

            return NoContent();
        }

    }
}
