using PlannerProjekt.Entities;

namespace PlannerProjekt.Dtos
{
    public class TaskDto
    {
        public int Id { get; set; }
        public bool IsCompleted { get; set; }
        public string Title { get; set; }
        public int CategoryId { get; set; } 
        public int SetTimeId { get; set; }
    }
}
