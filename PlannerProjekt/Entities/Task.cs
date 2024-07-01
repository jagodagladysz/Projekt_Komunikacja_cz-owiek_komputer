using System.ComponentModel.DataAnnotations.Schema;

namespace PlannerProjekt.Entities
{
    public class Task
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; } = false;
        public ICollection<SubTask> SubTasks { get; set; } // podzadania

        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category? Category { get; set; } // kategoria/tag/etykieta

        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; } // przypisanie do konta

        [ForeignKey("SetTimeId")]
        public SetTime SetTime { get; set; } // przypisanie do czasu pracy/wolnego/domyslnego
        public int SetTimeId { get; set; }
    
    }
}
