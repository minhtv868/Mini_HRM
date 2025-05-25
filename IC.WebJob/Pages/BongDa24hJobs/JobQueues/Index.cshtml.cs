using IC.Application.Features.BongDa24hJobs.JobQueues.DTOs;
using IC.Application.Features.BongDa24hJobs.JobQueues.Queries;
using IC.Shared;
using IC.WebJob.Models;
using Microsoft.AspNetCore.Mvc;

namespace IC.WebJob.Pages.BongDa24hJobs.JobQueues
{
    public class IndexModel : BasePageModel
    {
        [BindProperty]
        public JobQueueGetPageQuery Query { get; set; }
        public PaginatedResult<JobQueueGetPageDto> PaginatedResult;
        private readonly int _pageSize = 50;
        public async Task<IActionResult> OnGet(JobQueueGetPageQuery query, [FromQuery] int page = 1)
        {
            Query = query;
            Query.Page = page;
            Query.PageSize = _pageSize;
            PaginatedResult = await Mediator.Send(Query);
            PagingInput = new PagingInput(query.Page, query.PageSize, PaginatedResult.TotalPages);
            return Page();
        }
        public async Task<IActionResult> OnGetBindDataAsync(JobQueueGetPageQuery query, [FromQuery] int page = 1)
        {
            query.Page = page;
            query.PageSize = _pageSize;

            PaginatedResult = await Mediator.Send(query);
            PagingInput = new PagingInput(query.Page, query.PageSize, PaginatedResult.TotalPages);

            return Partial("BindData", Tuple.Create(PaginatedResult, PagingInput));
        }
    }
}
