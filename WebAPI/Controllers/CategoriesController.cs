using Microsoft.AspNetCore.Mvc;
using Web.Application.Features.Finance.Categories.Queries;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : BaseController
    {

        [HttpGet("GetAllBySite")]
        public async Task<IActionResult> GetAllBySite(short siteId)
        {
            var resultData = await Mediator.Send(new CategoryGetAllBySiteQuery() { SiteId = siteId });
            return Ok(resultData);
        }

    }
}
