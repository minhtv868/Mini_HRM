using IC.Application.Features.IdentityFeatures.SysFunctions.Queries;
using IC.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IC.WebJob.ViewComponents
{
	public class FunctionFavoriteViewComponent : ViewComponent
	{
		private readonly IMediator _mediator;
		private readonly ICurrentUserService _currentUserService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		public FunctionFavoriteViewComponent(IMediator mediator, ICurrentUserService currentUserService, IHttpContextAccessor httpContextAccessor)
		{
			_mediator = mediator;
			_currentUserService = currentUserService;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			SysFunctionGetByUrlDto sysFunctionGetMenuByUserDto = await _mediator.Send(new SysFunctionGetByUrlQuery
			{
				Url = _httpContextAccessor.HttpContext.Request.Path,
				UserId = _currentUserService.UserId
			});

			return View(sysFunctionGetMenuByUserDto);
		}
	}
}
