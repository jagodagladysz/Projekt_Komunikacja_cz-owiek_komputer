using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PlannerProjekt.Dtos;
using PlannerProjekt.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PlannerProjekt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorizationController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public AuthorizationController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto register)
        {
            if (_context.Users.Any(u => u.Login == register.Login))
            {
                return BadRequest("Username already exists.");
            }

            var user = new User
            {
                Login = register.Login,
                Password = BCrypt.Net.BCrypt.HashPassword(register.Password),
                Role = "user"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("Registration successful.");
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto login)
        {
            if (IsValidUser(login, out var role))
            {
                var token = GenerateJwtToken(login.Login, role);
                return Ok(new { token });
            }
            return Unauthorized();
        }


        [HttpPost("change-password")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePassword)
        {
            var username = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("Invalid token.");
            }

            var user = _context.Users.SingleOrDefault(u => u.Login == username);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (!BCrypt.Net.BCrypt.Verify(changePassword.OldPassword, user.Password))
            {
                return BadRequest("Old password is incorrect.");
            }

            user.Password = BCrypt.Net.BCrypt.HashPassword(changePassword.NewPassword);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            return Ok("Password changed successfully.");
        }


        private bool IsValidUser(LoginDto login, out string role)
        {
            role = null;
            var user = _context.Users.SingleOrDefault(u => u.Login == login.Login);

            if (user != null && BCrypt.Net.BCrypt.Verify(login.Password, user.Password))
            {
                role = user.Role;
                return true;
            }

            return false;
        }

        private string GenerateJwtToken(string username, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("yourSuperSecretKey1234567890123456"));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim(ClaimTypes.Role, role),
                new Claim(ClaimTypes.Name, username)
            };

            var token = new JwtSecurityToken(
                issuer: "yourIssuer",
                audience: "yourAudience",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: credentials);

            return "Bearer " + new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
