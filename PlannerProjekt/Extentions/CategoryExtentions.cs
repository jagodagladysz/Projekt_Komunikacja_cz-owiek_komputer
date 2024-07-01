using PlannerProjekt.Dtos;
using PlannerProjekt.Entities;

namespace PlannerProjekt.Extentions
{
    public static class CategoryExtentions
    {
        public static CategoryDto ToDto(this Category category)
        {
            if (category == null)
                return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }
        public static Category ToEntity(this CategoryDto categoryDto)
        {
            if (categoryDto == null)
                return null;

            return new Category
            {
                Id = categoryDto.Id,
                Name = categoryDto.Name
            };
        }
    }
}
