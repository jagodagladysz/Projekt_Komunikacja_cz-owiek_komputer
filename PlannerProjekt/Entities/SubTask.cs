using System.ComponentModel.DataAnnotations.Schema;

namespace PlannerProjekt.Entities
{
    public class SubTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public bool IsCompleted { get; set; } = false;

        public int TaskId { get; set; } 
        [ForeignKey("TaskId")]
        public Task Task { get; set; } = null!;
    }
}
