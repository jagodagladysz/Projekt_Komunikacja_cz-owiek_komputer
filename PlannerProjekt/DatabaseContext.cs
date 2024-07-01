using Microsoft.EntityFrameworkCore;
using PlannerProjekt.Entities;
using System.Collections.Generic;

namespace PlannerProjekt
{
    public class DatabaseContext : DbContext
    {
        private IConfiguration _configuration { get; }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<Entities.Task> Tasks { get; set; } = null!;
        public DbSet<SubTask> SubTasks { get; set; } = null!;
        public DbSet<SetTime> SetTimes { get; set; } = null!;

        public DatabaseContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // data
            modelBuilder.ApplyConfiguration(new Configurations.CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.SetTimeConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.UserConfiguration());

            // relations
            modelBuilder.Entity<Entities.Task>()
                .HasOne(t => t.Category)
                .WithMany(c => c.Tasks)
                .HasForeignKey(t => t.CategoryId);

            modelBuilder.Entity<Entities.Task>()
                 .HasOne(t => t.User)
                 .WithMany(u => u.Tasks)
                 .HasForeignKey(t => t.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SubTask>()
               .HasOne(st => st.Task)
               .WithMany(t => t.SubTasks)
               .HasForeignKey(st => st.TaskId)
               .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Entities.Task>() 
               .HasOne(t => t.SetTime)
               .WithMany()
               .HasForeignKey(t => t.SetTimeId)
               .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Categories)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=databse.db",
                x => x.MigrationsHistoryTable("__EFMigrationsHistory", "Planner"));

            options.LogTo(x => System.Diagnostics.Debug.WriteLine(x));
        }
    }
}
