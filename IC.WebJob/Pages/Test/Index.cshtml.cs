
using IC.Application.Jobs.ProcessCommons;
using MediatR;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IC.WebCMS.Pages.Test
{
    public class IndexModel : PageModel
    {
        private readonly IMediator _mediator;
        public IndexModel(IHttpContextAccessor httpContextAccessor, IMediator mediator)
        {
            _mediator = mediator;
        }
        public async Task OnGetAsync()
        {
            await _mediator.Send(new GoldPriceCrawlJob());
        }

    }
}
