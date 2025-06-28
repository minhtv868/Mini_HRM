using Microsoft.AspNetCore.Mvc;
using WebJob.Filters;

namespace WebJob.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ServiceFilter(typeof(ApiKeyFilter))]
    public class JobsController : ApiControllerBase
    {

    }
}
