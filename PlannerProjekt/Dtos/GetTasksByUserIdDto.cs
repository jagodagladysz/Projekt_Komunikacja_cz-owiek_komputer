namespace PlannerProjekt.Dtos
{
    public class GetTasksByUserIdDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public int CategoryId { get; set; }
        public int UserId { get; set; }
        public int? SetTimeId { get; set; }
    }
}
