using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebJob.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public abstract class ApiControllerBase : ControllerBase
    {
        private ISender _mediator = null;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}
