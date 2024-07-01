using Microsoft.EntityFrameworkCore;
using PlannerProjekt.Dtos;

namespace PlannerProjekt.Services
{
    public class SetTimeService
    {
        private readonly DatabaseContext _dbContext;

        public SetTimeService(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<SetTimeDto> GetSetTimeByIdAsync(int setId)
        {
            var setTime = await _dbContext.SetTimes.FindAsync(setId);
            if (setTime == null)
            {
                return null;
            }

            var setTimeDto = new SetTimeDto
            {
                Id = setTime.Id,
                TimeFrom = setTime.TimeFrom,
                TimeTo = setTime.TimeTo,
                Type = setTime.Type
            };

            return setTimeDto;
        }

        public async Task<SetTimeDto> UpdateSetTimeAsync(UpdateSetTimeDto setTimeDto)
        {
            var setTime = await _dbContext.SetTimes.FindAsync(setTimeDto.Id);
            if (setTime == null)
            {
                return null; 
            }

            setTime.TimeFrom = setTimeDto.TimeFrom;
            setTime.TimeTo = setTimeDto.TimeTo;


            _dbContext.SetTimes.Update(setTime);
            await _dbContext.SaveChangesAsync();

            var updatedSetTimeDto = new SetTimeDto
            {
                Id = setTime.Id,
                TimeFrom = setTime.TimeFrom,
                TimeTo = setTime.TimeTo,

            };

            return updatedSetTimeDto;
        }

        public async Task<List<GetTasksByUserIdDto>> GetTasksByUserIdAsync(int userId)
        {
            var tasks = await _dbContext.Tasks
                .Where(t => t.UserId == userId)
                .Select(t => new GetTasksByUserIdDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    IsCompleted = t.IsCompleted,
                    CategoryId = t.CategoryId,
                    UserId = t.UserId,
                    SetTimeId = t.SetTimeId
                })
                .ToListAsync();

            return tasks;
        }

    }
}
