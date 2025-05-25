using IC.Application.Features.BongDa24hCrawls.GetCountData;
using IC.Application.Features.BongDa24hCrawls.UrlCrawls.Commands;
using IC.Application.Features.BongDa24hJobs.JobQueues.Commands;
using IC.Application.Features.BongDa24hJobs.JobQueues.DTOs;
using IC.Application.Features.BongDa24hJobs.Jobs.DTOs;
using IC.Application.Jobs;
using IC.Domain.Enums.BongDa24hCrawls;
using IC.Shared;
using IC.WebJob.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace IC.WebJob.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ApiKeyFilter))]
    public class JobsController : ApiControllerBase
    {
        [HttpPost("/Job/RemapPlayerCareerTranferInjury")]
        public async Task<IActionResult> RemapPlayerCareerTranferInjury([FromBody] RemapPlayerCareerTranferInjuryDto remapPlayerCareerTranferInjuryDto)
        {
            if (remapPlayerCareerTranferInjuryDto == null || remapPlayerCareerTranferInjuryDto.DataSouceName.IsNullOrEmpty() || remapPlayerCareerTranferInjuryDto.Ids.Count == 0)
            {
                return BadRequest("Not Ok");
            }
            JobQueueCreateCommand command = new JobQueueCreateCommand
            {
                DataSouceName = remapPlayerCareerTranferInjuryDto.DataSouceName,
                DataId = remapPlayerCareerTranferInjuryDto.DataSouceName,
                JobName = JobClassTypeEnum.ProcessAsyncPlayerAgainByKeyJob,
                DataJson = JsonConvert.SerializeObject(remapPlayerCareerTranferInjuryDto.Ids)
            };
            var result = await Mediator.Send(command);
            if (result == 0)
            {
                return Ok("DataAny");
            }
            if (result > 0)
            {
                return Ok("Ok");
            }
            return BadRequest("Not Ok");
        }

        [HttpPost("/Job/ReCrawlByMatch")]
        public async Task<IActionResult> ReCrawlByMatch([FromBody] int matchId)
        {
            if (matchId > 0)
            {
                JobQueueCreateCommand command = new JobQueueCreateCommand();
                command.DataSouceName = DataSourceNameEnum.ReCrawlByMatchs;
                command.DataId = JsonConvert.SerializeObject(matchId);
                command.JobName = JobClassTypeEnum.ProcessGetPlayerRefreshDataByMatchJob;
                var result = await Mediator.Send(command);
                if (result == 0)
                {
                    return Ok("DataAny");
                }
                if (result > 0)
                {
                    return Ok("Ok");
                }
                return BadRequest("Not Ok");
            }
            return BadRequest("Not Ok");
        }

        [HttpPost("/Job/ReCrawlByPlayer")]
        public async Task<IActionResult> ReCrawlByPlayer([FromBody] int playerId)
        {
            if (playerId > 0)
            {
                UrlReCrawlByPlayerCommand command = new UrlReCrawlByPlayerCommand();
                command.PlayerId = playerId;
                var result = await Mediator.Send(command);
                if (result.Succeeded)
                {
                    return Ok("Ok");
                }
                return BadRequest("Not Ok");
            }
            return BadRequest("Not Ok");
        }
        [HttpPost("/Job/DeleteCareerTranferInjuryCrawlByPlayer")]
        public async Task<IActionResult> DeleteCareerTranferInjuryCrawlByPlayer([FromBody] int playerId)
        {
            if (playerId > 0)
            {
                JobQueueCreateCommand command = new JobQueueCreateCommand();
                command.DataSouceName = DataSourceNameEnum.Players; ;
                command.DataId = JsonConvert.SerializeObject(playerId);
                command.JobName = JobClassTypeEnum.ProcessDeleteCITCrawlByPlayerJob;
                var result = await Mediator.Send(command);
                if (result == 0)
                {
                    return Ok("DataAny");
                }
                if (result > 0)
                {
                    return Ok("Ok");
                }
                return BadRequest("Not Ok");
            }
            return BadRequest("Not Ok");
        }
        [AllowAnonymous]
        [HttpGet("/Job/GetErrorFSPlayerCrawl")]
        public async Task<IActionResult> GetErrorFSPlayerCrawl()
        {
            return Ok(await Mediator.Send(new GetErrorFSPlayerCrawlQuery()));
        }
        [AllowAnonymous]
        [HttpGet("/Job/PlayerCareerInjuryHistoryTransferHistoryCrawl")]
        public async Task<IActionResult> GetErrorPlayerCareerInjuryHistoryTransferHistoryCrawl()
        {
            return Ok(await Mediator.Send(new GetErrorPlayerCareerInjuryHistoryTransferHistoryCrawlQuery()));
        }
    }
}
