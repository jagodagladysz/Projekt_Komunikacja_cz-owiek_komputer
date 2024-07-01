namespace PlannerProjekt.Dtos
{
    public class GetAllTasksDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public int CategoryId { get; set; }
    }
}
