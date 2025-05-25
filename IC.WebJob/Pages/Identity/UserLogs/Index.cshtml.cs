using IC.Application.Features.IdentityFeatures.UserLogs.Queries;
using IC.Shared;
using IC.WebJob.Helpers.Configs;
using IC.WebJob.Helpers.Extensions;
using IC.WebJob.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IC.WebJob.Pages.Identity.UserLogs
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
