using Microsoft.AspNetCore.Mvc;
using Web.Application.Features.Finance.Articles.Queries;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticleController : BaseController
    {
        [HttpGet("GetAllBySite")]
        public async Task<IActionResult> GetAllBySite(short siteId)
        {
            var resultData = await Mediator.Send(new ArticleGetAllBySiteQuery() { SiteId = siteId });
            return Ok(resultData);
        }
        [HttpGet("GetPage")]
        public async Task<IActionResult> GetPage([FromQuery] ArticleGetPageQuery query)
        {
            var resultData = await Mediator.Send(query);
            return Ok(resultData);
        }
        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var resultData = await Mediator.Send(new ArticleGetByIdQuery() { ArticleId = id });
            return Ok(resultData);
        }

    }
}
