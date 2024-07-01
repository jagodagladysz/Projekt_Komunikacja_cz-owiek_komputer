using PlannerProjekt.Dtos;

namespace PlannerProjekt.Extentions
{
    public static class TaskExtention
    {
        public static TaskDto ToDto(this Entities.Task task)
        {
            return new TaskDto
            {
                Title = task.Title,
                CategoryId = task.CategoryId
 
            };
        }

        public static Entities.Task ToEntity(this TaskDto dto, int userId)
        {
            var task = new Entities.Task
            {
                Title = dto.Title,
                IsCompleted = false,
                CategoryId = dto.CategoryId,
                UserId = userId,
                SetTimeId = dto.SetTimeId
            };

            task.CategoryId = dto.CategoryId <= 0 ? 1 : dto.CategoryId;
            task.SetTimeId = dto.SetTimeId <= 0 ? 1 : dto.SetTimeId;

            return task;
        }
    }
}
