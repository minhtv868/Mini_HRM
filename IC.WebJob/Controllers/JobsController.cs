using IC.WebJob.Filters;
using Microsoft.AspNetCore.Mvc;

namespace IC.WebJob.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ApiKeyFilter))]
    public class JobsController : ApiControllerBase
    {

    }
}
