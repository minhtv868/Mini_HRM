using Microsoft.AspNetCore.Mvc;
using Web.Application.Features.Finance.Attendances.Commands;
using Web.Application.Features.Finance.Attendances.DTOs;
using Web.Application.Features.Finance.Attendances.Queries;
using Web.Application.Features.Finance.Sites.DTOs;
using Web.Application.Features.Finance.Sites.Queries;
using Web.Shared;
using WebJob.Helpers.Configs;
using WebJob.Models;

namespace WebJob.Pages.HRM.Attendances
{
    public class IndexModel : BasePageModel
    {
        [BindProperty]
        public AttendanceGetPageQuery Query { get; set; }
        public PaginatedResult<AttendanceGetPageDto> PaginatedResult;
        public List<SiteGetAllByUserDto> SiteList;
        private readonly int _pageSize = AppConfig.AppSettings.PageSize;
        public async Task<IActionResult> OnGet(AttendanceGetPageQuery query, [FromQuery] int page = 1)
        {
            Query = query;
            Query.Page = page;
            Query.PageSize = _pageSize;
            SiteList = await Mediator.Send(new SiteGetAllByUserQuery());
            if (SiteList?.Any() == true)
            {
                if (!Query.SiteId.HasValue || Query.SiteId <= 0)
                {
                    Query.SiteId = (HttpContext.Session.GetInt32("SiteId"))
                                   ?? SiteList.First().SiteId;
                }
                else
                {
                    HttpContext.Session.SetInt32("SiteId", Query.SiteId.Value);
                }
            }
            PaginatedResult = await Mediator.Send(query);
            PagingInput = new PagingInput(query.Page, query.PageSize, PaginatedResult.TotalPages);
            return Page();
        }
        public async Task<IActionResult> OnGetBindDataAsync(AttendanceGetPageQuery query, [FromQuery] int page = 1)
        {
            SiteList = await Mediator.Send(new SiteGetAllByUserQuery());
            var siteId = HttpContext.Session.GetInt32("SiteId");
            query.SiteId ??= siteId > 0
                ? siteId
                : SiteList?.FirstOrDefault()?.SiteId;
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

            var deleteResult = await Mediator.Send(new AttendanceDeleteCommand { AttendanceId = id });

            return new AjaxResult
            {
                Id = id.ToString(),
                Succeeded = deleteResult.Succeeded,
                Messages = deleteResult.Messages
            };
        }
    }
}
