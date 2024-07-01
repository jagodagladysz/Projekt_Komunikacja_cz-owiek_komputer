using Microsoft.EntityFrameworkCore;
using PlannerProjekt.Dtos;
using PlannerProjekt.Entities;
using PlannerProjekt.Extentions;

namespace PlannerProjekt.Services
{
    public class PlannerService
    {
        private readonly DatabaseContext _dbContext;
        public PlannerService(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users.SingleOrDefaultAsync(u => u.Login == username);
        }

        public async Task<Entities.Task> AddTaskAsync(TaskDto taskDto, int userId)
        {
            var task = taskDto.ToEntity(userId);

            _dbContext.Tasks.Add(task);
            await _dbContext.SaveChangesAsync();

            return task;
        }


        public async Task<Entities.Task> UpdateTaskAsync(int taskId, TaskDto updatedTaskDto, User user)
        {
            var task = await _dbContext.Tasks.SingleOrDefaultAsync(t => t.Id == taskId && t.UserId == user.Id);

            if (task == null)
            {
                return null;
            }
            task.IsCompleted = updatedTaskDto.IsCompleted;
            task.Title = updatedTaskDto.Title;
            task.CategoryId = updatedTaskDto.CategoryId;
            task.SetTimeId = updatedTaskDto.SetTimeId;

            _dbContext.Tasks.Update(task);
            await _dbContext.SaveChangesAsync();

            return new Entities.Task
            {
                Id = task.Id,
                Title = task.Title,
                IsCompleted = task.IsCompleted,
                CategoryId = task.CategoryId,
                SetTimeId = task.SetTimeId
            };
        }


        public async Task<List<TaskDto>> GetTasksAsync(User user)
        {
            var tasks = await _dbContext.Tasks
                .Where(t => t.UserId == user.Id)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    IsCompleted = t.IsCompleted,
                    CategoryId = t.CategoryId
                })
                .ToListAsync();

            return tasks;
        }

        public async Task<TaskDto> GetTaskByIdAsync(int taskId, string username)
        {
            var user = await GetUserByUsernameAsync(username);

            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            var task = await _dbContext.Tasks
                .Where(t => t.Id == taskId && t.UserId == user.Id)
                .Select(t => new TaskDto
                {
                    Title = t.Title,
                    CategoryId = t.CategoryId
                })
                .FirstOrDefaultAsync();

            return task;
        }


        public async Task<bool> DeleteSubTaskAsync(int subTaskId, User user)
        {
            var subTask = await _dbContext.SubTasks
                .Include(st => st.Task)
                .SingleOrDefaultAsync(st => st.Id == subTaskId && st.Task.UserId == user.Id);

            if (subTask == null)
            {
                return false;
            }

            _dbContext.SubTasks.Remove(subTask);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        // isCompleted
        public async Task<Entities.Task> MarkTaskAsCompletedAsync(int taskId, User user)
        {
            var task = await _dbContext.Tasks
                .SingleOrDefaultAsync(t => t.Id == taskId && t.UserId == user.Id);

            if (task == null)
            {
                return null;
            }

            task.IsCompleted = true;
            _dbContext.Tasks.Update(task);
            await _dbContext.SaveChangesAsync();

 
            return new Entities.Task
            {
                Id = task.Id,
                Title = task.Title,
                IsCompleted = task.IsCompleted,
                CategoryId = task.CategoryId,
                SetTimeId = task.SetTimeId 
            };
        }

        public async Task<List<TaskDto>> GetCompletedTasksAsync(User user)
        {
            var tasks = await _dbContext.Tasks
                .Where(t => t.UserId == user.Id && t.IsCompleted)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    IsCompleted = t.IsCompleted,
                    CategoryId = t.CategoryId,
                    SetTimeId = t.SetTimeId
                })
                .ToListAsync();

            return tasks;
        }

        public async Task<List<TaskDto>> GetIncompleteTasksAsync(User user)
        {
            var tasks = await _dbContext.Tasks
                .Where(t => t.UserId == user.Id && !t.IsCompleted)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    IsCompleted = t.IsCompleted,
                    CategoryId = t.CategoryId,
                    SetTimeId = t.SetTimeId
                })
                .ToListAsync();

            return tasks;
        }

        public async Task<SubTask> AddSubTaskAsync(SubTaskDto subTaskDto, int taskId, string username)
        {
            var user = await GetUserByUsernameAsync(username);
            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            var task = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.Id == taskId && t.UserId == user.Id);
            if (task == null)
            {
                throw new KeyNotFoundException("Task not found.");
            }

            var subTask = subTaskDto.ToEntity(taskId);
            _dbContext.SubTasks.Add(subTask);
            await _dbContext.SaveChangesAsync();

            return subTask;
        }

        public async Task<IEnumerable<SubTask>> GetSubTasksForTaskAsync(int taskId, string username)
        {
            var user = await GetUserByUsernameAsync(username);

            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            return await _dbContext.SubTasks
                .Where(st => st.TaskId == taskId)
                .ToListAsync();
        }

        public async Task<TaskWithSubTasksDto> GetTaskWithSubTasksAsync(int taskId, User user)
        {
            var task = await _dbContext.Tasks
                .Include(t => t.SubTasks)
                .SingleOrDefaultAsync(t => t.Id == taskId && t.UserId == user.Id);

            if (task == null)
            {
                return null;
            }

            var taskWithSubTasksDto = new TaskWithSubTasksDto
            {
                Id = task.Id,
                Title = task.Title,
                IsCompleted = task.IsCompleted,
                CategoryId = task.CategoryId,
                SetTimeId = task.SetTimeId,
                SubTasks = task.SubTasks.Select(st => new SubTaskDto
                {
                    Id = st.Id,
                    Title = st.Title,
                    IsCompleted = st.IsCompleted
                }).ToList()
            };

            return taskWithSubTasksDto;
        }

        public async Task<bool> DeleteTaskAsync(int taskId, User user)
        {
            var task = await _dbContext.Tasks
                .Include(t => t.SubTasks)
                .SingleOrDefaultAsync(t => t.Id == taskId && t.UserId == user.Id);

            if (task == null)
            {
                return false;
            }

            if (task.SubTasks.Any())
            {
                _dbContext.SubTasks.RemoveRange(task.SubTasks);
            }

            _dbContext.Tasks.Remove(task);

            await _dbContext.SaveChangesAsync();

            return true;
        }

    }
}
