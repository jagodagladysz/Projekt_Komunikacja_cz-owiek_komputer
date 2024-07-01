using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PlannerProjekt.Dtos;
using PlannerProjekt.Services;

namespace PlannerProjekt.Controllers
{
    [ApiController]
    [Route("api/set-times")]
    [Authorize(Policy = "UserPolicy")]
    public class SetTimeController : ControllerBase
    {
        private readonly SetTimeService _setTimeService;

        public SetTimeController(SetTimeService setTimeService)
        {
            _setTimeService = setTimeService;
        }

        [HttpGet("get-set-time/{setId}")]
        public async Task<IActionResult> GetSetTime(int setId)
        {
            var setTimeDto = await _setTimeService.GetSetTimeByIdAsync(setId);
            if (setTimeDto == null)
            {
                return NotFound();
            }

            return Ok(setTimeDto);
        }
    }
}
