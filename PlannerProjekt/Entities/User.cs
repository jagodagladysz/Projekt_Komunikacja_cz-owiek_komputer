using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace PlannerProjekt.Entities
{
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Login { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();

    }
}
