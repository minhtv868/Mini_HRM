using Microsoft.AspNetCore.Mvc;
using Web.Application.Features.Finance.Matchs.Queries;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : BaseController
    {
        [HttpGet("GetAllSchedule")]
        public async Task<IActionResult> GetAllSchedule(
        [FromQuery] short? leagueId, [FromQuery] string leagueUrl,
        [FromQuery] DateTime? estimateStartTime)
        {
            var query = new MatchGetScheduleQuery
            {
                LeagueId = leagueId,
                LeagueUrl = leagueUrl,
                EstimateStartTime = estimateStartTime
            };

            var resultData = await Mediator.Send(query);
            return Ok(resultData);
        }

    }
}
