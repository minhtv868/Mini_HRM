using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : Controller
    {

        private ISender _mediator = null;
        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        private IMapper _mapper = null;
        protected IMapper Mapper => _mapper ??= HttpContext.RequestServices.GetRequiredService<IMapper>();
        private ILogger? _logger;
        protected ILogger Logger =>
                    _logger ??= HttpContext.RequestServices.GetRequiredService<ILogger<BaseController>>();
        protected CancellationToken CancellationToken => HttpContext.RequestAborted;
    }
}
