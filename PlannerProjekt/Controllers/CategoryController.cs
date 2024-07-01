using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlannerProjekt.Dtos;
using PlannerProjekt.Entities;
using PlannerProjekt.Extentions;
using PlannerProjekt.Services;
using System.Security.Claims;

namespace PlannerProjekt.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Policy = "UserPolicy")]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService _categoryService;

        public CategoryController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        private string GetUsernameFromToken()
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
            {
                throw new UnauthorizedAccessException("Invalid token.");
            }
            return username;
        }

        [HttpPost("create-category")]
        [Authorize(Policy = "UserPolicy")] 
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var username = GetUsernameFromToken(); 

            var createdCategory = await _categoryService.CreateCategoryAsync(categoryDto, username); 

            if (createdCategory == null)
            {
                return BadRequest("Failed to create category.");
            }

            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, createdCategory);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var username = GetUsernameFromToken();

            var categoryDto = await _categoryService.GetCategoryByIdAsync(id);

            if (categoryDto == null)
            {
                return NotFound("Category not found.");
            }

            return Ok(categoryDto);
        }

        [HttpGet("get-all-categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var username = GetUsernameFromToken();

            var categories = await _categoryService.GetAllCategoriesAsync(username);

            return Ok(categories);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var username = GetUsernameFromToken();

            var result = await _categoryService.DeleteCategoryAsync(id);

            if (!result)
            {
                return NotFound("Category not found or could not be deleted.");
            }

            return NoContent();
        }
    }
}
