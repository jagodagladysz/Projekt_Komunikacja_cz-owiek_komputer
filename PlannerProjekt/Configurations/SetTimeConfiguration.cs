using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using PlannerProjekt.Entities;

namespace PlannerProjekt.Configurations
{
    public class SetTimeConfiguration : IEntityTypeConfiguration<SetTime>
    {
        public void Configure(EntityTypeBuilder<SetTime> builder)
        {
            builder.HasData(
                new SetTime
                {
                    Id = 1,
                    TimeFrom = DateTime.Now,
                    TimeTo = DateTime.Now,
                    Type = "Default"
                },

                 new SetTime
                 {
                     Id = 2,
                     TimeFrom = DateTime.Today.AddHours(8).Date + new TimeSpan(8, 0, 0),
                     TimeTo = DateTime.Today.AddHours(8).Date + new TimeSpan(16, 0, 0),
                     Type = "WorkTime"
                 },

                  new SetTime
                  {
                      Id = 3,
                      TimeFrom = DateTime.Today.AddHours(8).Date + new TimeSpan(18, 0, 0), 
                      TimeTo = DateTime.Today.AddHours(8).Date + new TimeSpan(21, 0, 0),
                      Type = "FreeTime"
                  }
            );
        }

    }
}

