using System.ComponentModel.DataAnnotations;

namespace PlannerProjekt.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
