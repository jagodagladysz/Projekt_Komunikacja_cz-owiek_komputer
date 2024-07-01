namespace PlannerProjekt.Dtos
{
    public class TaskWithSubTasksDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public int CategoryId { get; set; }
        public int? SetTimeId { get; set; }
        public List<SubTaskDto> SubTasks { get; set; }
    }
}
