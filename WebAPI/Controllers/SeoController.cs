using Microsoft.AspNetCore.Mvc;
using Web.Application.Features.Finance.Seos.Queries;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeoController : BaseController
    {
        [HttpGet("GetSeoByUrl")]
        public async Task<IActionResult> GetSeoByUrl([FromQuery] string seoUrl)
        {
            var query = new SeoGetByUrlQuery
            {
                Url = seoUrl
            };

            var resultData = await Mediator.Send(query);
            return Ok(resultData);
        }
    }
}
