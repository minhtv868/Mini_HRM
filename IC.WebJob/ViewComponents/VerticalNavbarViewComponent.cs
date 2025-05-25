using IC.Application.Features.IdentityFeatures.SysFunctions.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using IC.Infrastructure.Extensions;

namespace IC.WebJob.ViewComponents
{
	public class VerticalNavbarViewComponent : ViewComponent
	{
		private readonly IMediator _mediator;

		public VerticalNavbarViewComponent(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var sysFunctionsListByUser = await _mediator.Send(new SysFunctionGetMenuByUserQuery(HttpContext.User.GetUserId()));

			return View(sysFunctionsListByUser);
		}
	}
}
