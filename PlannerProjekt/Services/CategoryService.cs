using Microsoft.EntityFrameworkCore;
using PlannerProjekt.Dtos;
using PlannerProjekt.Entities;
using PlannerProjekt.Extentions;

namespace PlannerProjekt.Services
{
    public class CategoryService
    {
        private readonly DatabaseContext _dbContext;

        public CategoryService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CategoryDto categoryDto, string username)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Login == username);

            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            var category = new Category
            {
                Name = categoryDto.Name,
                UserId = user.Id
            };

            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }


        public async Task<CategoryDto> GetCategoryByIdAsync(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);

            if (category == null)
            {
                return null;
            }

            return category.ToDto();
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(string username)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(u => u.Login == username);

            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            var categories = await _dbContext.Categories
                .Where(c => c.UserId == user.Id)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();

            return categories;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);

            if (category == null)
            {
                return false;
            }

            var tasksToUpdate = await _dbContext.Tasks.Where(t => t.CategoryId == category.Id).ToListAsync();
            foreach (var task in tasksToUpdate)
            {
                task.CategoryId = 1; 
            }

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();

            return true;
        }

    }
}
