using Microsoft.AspNetCore.Mvc;
using Web.Application.Features.Finance.Departments.Commands;
using Web.Application.Features.Finance.Departments.DTOs;
using Web.Application.Features.Finance.Departments.Queries;
using Web.Application.Features.Finance.Sites.Queries;
using Web.Shared;
using WebJob.Helpers.Configs;
using WebJob.Models;

namespace WebJob.Pages.Finance.Departments
{
    public class IndexModel : BasePageModel
    {
        [BindProperty]
        public DepartmentGetPageQuery Query { get; set; }
        public PaginatedResult<DepartmentGetPageDto> PaginatedResult;
        private readonly int _pageSize = AppConfig.AppSettings.PageSize;
        public async Task<IActionResult> OnGet(DepartmentGetPageQuery query, [FromQuery] int page = 1)
        {
            Query = query;
            Query.Page = page;
            Query.PageSize = _pageSize;
            SiteList = await Mediator.Send(new SiteGetAllByUserQuery());
            Query.SiteId ??= await GetOrSetSiteIdAsync();
            PaginatedResult = await Mediator.Send(query);
            PagingInput = new PagingInput(query.Page, query.PageSize, PaginatedResult.TotalPages);
            return Page();
        }
        public async Task<IActionResult> OnGetBindDataAsync(DepartmentGetPageQuery query, [FromQuery] int page = 1)
        {
            SiteList = await Mediator.Send(new SiteGetAllByUserQuery());
            Query.SiteId ??= await GetOrSetSiteIdAsync();
            query.Page = page;

            query.PageSize = _pageSize;
            PaginatedResult = await Mediator.Send(query);
            PagingInput = new PagingInput(query.Page, query.PageSize, PaginatedResult.TotalPages);
            SiteList = await Mediator.Send(new SiteGetAllByUserQuery());
            return Partial("BindData", Tuple.Create(PaginatedResult, PagingInput, query.SiteId));
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id = 0)
        {
            if (id <= 0)
            {
                return new AjaxResult
                {
                    Succeeded = false,
                    Messages = new List<string> { $"Phòng ban không tồn tại." }
                };
            }

            var deleteResult = await Mediator.Send(new DepartmentDeleteCommand { DepartmentId = id });

            return new AjaxResult
            {
                Id = id.ToString(),
                Succeeded = deleteResult.Succeeded,
                Messages = deleteResult.Messages
            };
        }
    }
}
