using Web.Application.Features.BongDa24hJobs.Jobs.Commands;
using Web.Application.Features.BongDa24hJobs.Jobs.DTOs;
using Web.Application.Features.BongDa24hJobs.Jobs.Queries;
using Web.Shared;
using WebJob.Helpers.Configs;
using WebJob.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebJob.Pages.WebJobs.Jobs
{
    public class IndexModel : BasePageModel
    {
        [BindProperty]
        public JobGetByPageQuery Query { get; set; }

        public PaginatedResult<JobGetByPageDto> PaginatedResult { set; get; }

        private readonly int _pageSize = AppConfig.AppSettings.PageSize;

        public async Task<IActionResult> OnGetAsync(JobGetByPageQuery query, [FromQuery] int page = 1)
        {

            Query = query;

            Query.Page = page;

            Query.PageSize = _pageSize;

            PaginatedResult = await Mediator.Send(Query);

            PagingInput = new PagingInput(Query.Page, Query.PageSize, PaginatedResult.TotalPages);

            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id = 0)
        {
            if (id <= 0)
            {
                return new AjaxResult
                {
                    Succeeded = false,
                    Messages = new List<string> { $"Dữ liệu Id ${id} không tồn tại." }
                };
            }

            var deleteResult = await Mediator.Send(new JobDeleteCommand { Id = id });

            return new AjaxResult
            {
                Id = id.ToString(),
                Succeeded = deleteResult.Succeeded,
                Messages = deleteResult.Messages
            };
        }

        public async Task<IActionResult> OnGetBindDataAsync(JobGetByPageQuery query, [FromQuery] int page = 1)
        {
            query.Page = page;

            query.PageSize = _pageSize;

            PaginatedResult = await Mediator.Send(query);

            PagingInput = new PagingInput(query.Page, query.PageSize, PaginatedResult.TotalPages);

            return Partial("BindData", Tuple.Create(PaginatedResult, PagingInput));
        }
    }
}
