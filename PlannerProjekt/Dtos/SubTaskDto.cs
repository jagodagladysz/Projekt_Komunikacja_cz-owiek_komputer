namespace PlannerProjekt.Dtos
{
    public class SubTaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public int TaskId { get; set; }
    }
}
