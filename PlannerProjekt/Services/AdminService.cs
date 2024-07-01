using Microsoft.EntityFrameworkCore;
using PlannerProjekt.Dtos;

namespace PlannerProjekt.Services
{
    public class AdminService
    {
        private readonly DatabaseContext _dbContext;
        public AdminService(DatabaseContext databaseContext)
        {
            _dbContext = databaseContext;
        }

        public async Task<List<UserDto>> GetAllUsersWithRoleUserAsync()
        {
            var users = await _dbContext.Users
                .Where(u => u.Role == "user")
                .Select(u => new UserDto { Id = u.Id, Login = u.Login, Role = u.Role })
                .ToListAsync();

            return users;
        }
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateUserLoginAsync(int id, string newLogin)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            if (_dbContext.Users.Any(u => u.Login == newLogin))
            {
                throw new InvalidOperationException("Login already exists.");
            }

            user.Login = newLogin;
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}
