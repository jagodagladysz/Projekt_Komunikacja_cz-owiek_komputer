using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlannerProjekt.Dtos;
using PlannerProjekt.Services;

namespace PlannerProjekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AdminService _adminService;
        public AdminController(AdminService adminServie)
        {
            _adminService = adminServie;
        }

        [HttpGet("users")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> GetAllUsersWithRoleUser()
        {
            var users = await _adminService.GetAllUsersWithRoleUserAsync();
            return Ok(users);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _adminService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound("User not found.");
            }

            return Ok("User deleted successfully.");
        }

        [HttpPut("update-login/{id}")]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<IActionResult> UpdateUserLogin(int id, [FromBody] UpdateUserDto updateLoginDto)
        {
            try
            {
                var result = await _adminService.UpdateUserLoginAsync(id, updateLoginDto.NewLogin);
                if (!result)
                {
                    return NotFound("User not found.");
                }

                return Ok("User login updated successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
