using Web.Application.Features.IdentityFeatures.UserLogs.Queries;
using Web.Shared;
using WebJob.Helpers.Configs;
using WebJob.Helpers.Extensions;
using WebJob.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebJob.Pages.Identity.UserLogs
{
	public class IndexModel : BasePageModel
	{
		[BindProperty]
		public UserLogGetPageQuery Query { get; set; }

		public PaginatedResult<UserLogGetPageDto> PaginatedResult;

		private readonly int _pageSize = AppConfig.AppSettings.PageSize;

		public async Task<IActionResult> OnGetAsync(UserLogGetPageQuery query, [FromQuery] int page = 1)
		{
			Query = query;
			Query.Page = page;
			Query.PageSize = _pageSize;

			PaginatedResult = await Mediator.Send(Query);

			PagingInput = new PagingInput(Query.Page, Query.PageSize, PaginatedResult.TotalPages);

			return Page();
		}
	}
}
