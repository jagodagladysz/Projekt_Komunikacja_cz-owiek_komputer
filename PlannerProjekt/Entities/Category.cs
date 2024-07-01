using Microsoft.Extensions.Hosting;

namespace PlannerProjekt.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public int UserId { get; set; } 
        public User User { get; set; }
    }
}
