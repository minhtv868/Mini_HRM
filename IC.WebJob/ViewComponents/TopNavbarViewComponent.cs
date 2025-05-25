using IC.Application.Features.IdentityFeatures.SysFunctions.Queries;
using IC.Infrastructure.Extensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IC.WebJob.ViewComponents
{
	public class TopNavbarViewComponent : ViewComponent
	{
		private readonly IMediator _mediator;

		public TopNavbarViewComponent(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var sysFunctionsListByUser = await _mediator.Send(new SysFunctionGetMenuByUserQuery(HttpContext.User.GetUserId(), true));

			return View(sysFunctionsListByUser);
		}
	}
}
