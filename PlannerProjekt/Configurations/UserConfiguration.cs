using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PlannerProjekt.Entities;

namespace PlannerProjekt.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasData(
                new User
                {
                    Id = 1,
                    Login = "admin",
                    Password = BCrypt.Net.BCrypt.HashPassword("admin"),
                    Role = "admin"
                }
            );
        }
    }
}
