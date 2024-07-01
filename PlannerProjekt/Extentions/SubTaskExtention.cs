using PlannerProjekt.Dtos;
using PlannerProjekt.Entities;

namespace PlannerProjekt.Extentions
{
    public static class SubTaskExtention
    {
        public static SubTask ToEntity(this SubTaskDto subTaskDto, int taskId)
        {
            return new SubTask
            {
                Title = subTaskDto.Title,
                TaskId = taskId
            };
        }

        public static SubTaskDto ToDto(this SubTask subTask)
        {
            return new SubTaskDto
            {
                Id = subTask.Id,
                Title = subTask.Title,
                TaskId = subTask.TaskId 
            };
        }

    }
}
