using Microsoft.AspNetCore.Mvc;
using Web.Application.Features.Finance.Leagues.DTOs;
using Web.Application.Features.Finance.Leagues.Queries;
using Web.Application.Features.Finance.Matchs.Commands;
using Web.Application.Features.Finance.Matchs.DTOs;
using Web.Application.Features.Finance.Matchs.Queries;
using Web.Application.Features.Finance.Teams.DTOs;
using Web.Application.Features.Finance.Teams.Queries;
using Web.Shared;
using WebJob.Helpers.Configs;
using WebJob.Models;

namespace WebJob.Pages.Finance.Matchs
{
    public class IndexModel : BasePageModel
    {
        [BindProperty]
        public MatchGetPageQuery Query { get; set; }
        public PaginatedResult<MatchGetPageDto> PaginatedResult;
        public List<LeagueGetAllDto> LeagueList;
        public List<TeamGetAllDto> TeamList;
        private readonly int _pageSize = AppConfig.AppSettings.PageSize;
        public async Task<IActionResult> OnGet(MatchGetPageQuery query, [FromQuery] int page = 1)
        {
            Query = query;
            Query.Page = page;
            Query.PageSize = _pageSize;
            LeagueList = await Mediator.Send(new LeagueGetAllQuery());
            TeamList = await Mediator.Send(new TeamGetAllQuery());
            PaginatedResult = await Mediator.Send(query);
            PagingInput = new PagingInput(query.Page, query.PageSize, PaginatedResult.TotalPages);
            return Page();
        }
        public async Task<IActionResult> OnGetBindDataAsync(MatchGetPageQuery query, [FromQuery] int page = 1)
        {
            query.Page = page;

            query.PageSize = _pageSize;

            PaginatedResult = await Mediator.Send(query);

            PagingInput = new PagingInput(query.Page, query.PageSize, PaginatedResult.TotalPages);

            return Partial("BindData", Tuple.Create(PaginatedResult, PagingInput));
        }
        public async Task<IActionResult> OnPostBulkActionsAsync(string chkActionIds = "")
        {
            if (string.IsNullOrWhiteSpace(chkActionIds))
            {
                return new AjaxResult
                {
                    Succeeded = false,
                    Messages = new List<string> { "Vui lòng chọn Match cần thao tác" }
                };
            }
            ;

            var selectedIds = chkActionIds?.Split(',')?.Select(int.Parse)?.ToList();

            foreach (int id in selectedIds)
            {
                await Mediator.Send(new MatchDeleteCommand { MatchId = (short)id });
            }
            return new AjaxResult
            {
                Succeeded = true,
                Messages = new List<string> { "Thành công" }
            };
        }
        public async Task<IActionResult> OnPostDeleteAsync(short id = 0)
        {
            if (id <= 0)
            {
                return new AjaxResult
                {
                    Succeeded = false,
                    Messages = new List<string> { $"Match không t?n t?i." }
                };
            }

            var deleteResult = await Mediator.Send(new MatchDeleteCommand { MatchId = id });

            return new AjaxResult
            {
                Id = id.ToString(),
                Succeeded = deleteResult.Succeeded,
                Messages = deleteResult.Messages
            };
        }
    }
}
