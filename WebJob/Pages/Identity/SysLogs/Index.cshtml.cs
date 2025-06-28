using Web.Application.Features.IdentityFeatures.SysLogs.Queries;
using Web.Shared;
using WebJob.Helpers.Configs;
using WebJob.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebJob.Pages.Identity.SysLogs
{
	public class IndexModel : BasePageModel
    {
		[BindProperty]
		public SysLogGetPageQuery Query { get; set; }

		public PaginatedResult<SysLogGetPageDto> PaginatedResult;

		private readonly int _pageSize = AppConfig.AppSettings.PageSize;

		public async Task<IActionResult> OnGetAsync(SysLogGetPageQuery query, [FromQuery] int page = 1)
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
