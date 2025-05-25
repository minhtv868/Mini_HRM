using IC.Application.Features.IdentityFeatures.Users.Commands;
using IC.Application.Features.IdentityFeatures.Users.Queries;
using IC.Shared;
using IC.WebJob.Helpers.Configs;
using IC.WebJob.Models;
using Microsoft.AspNetCore.Mvc;

namespace IC.WebJob.Pages.Identity.Users
{
	public class IndexModel : BasePageModel
    {
        [BindProperty]
        public UserGetPageQuery Query { get; set; }

        public PaginatedResult<UserGetPageDto> PaginatedResult;

		private readonly int _pageSize = AppConfig.AppSettings.PageSize;

        public async Task<IActionResult> OnGetAsync(UserGetPageQuery query, [FromQuery] int page = 1)
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
                    Messages = new List<string> { $"Tài khoản Id <b>{id}</b> không tồn tại." }
                };
            }

            var deleteResult = await Mediator.Send(new UserDeleteCommand { Id = id });

            return new AjaxResult
            {
                Id = id.ToString(),
                Succeeded = deleteResult.Succeeded,
                Messages = deleteResult.Messages
            };
        }

        public async Task<IActionResult> OnPostBulkActionsAsync(string chkActionIds = "", string handler = "")
        {
            if (string.IsNullOrWhiteSpace(chkActionIds))
            {
                return new AjaxResult
                {
                    Succeeded = false,
                    Messages = new List<string> { "Vui lòng chọn tài khoản cần thao tác." }
                };
            };

            var selectedUserIds = chkActionIds?.Split(',')?.Select(int.Parse)?.ToList();

            foreach (int id in selectedUserIds)
            {
                if(Query.IsEnabled > 0)
                {
                    await Mediator.Send(new UserSetIsEnableCommand { Id = id, IsEnable = Query.IsEnabled == 1 });
				}
				else if(Query.TwoFactorEnabled > 0)
                {
					await Mediator.Send(new UserSetTwoFactorEnabledCommand { Id = id, TwoFactorEnabled = Query.TwoFactorEnabled == 1 });
				}
            }

            return new AjaxResult
            {
                Succeeded = true,
                Messages = new List<string> { "Cập nhật trạng thái tài khoản thành công." }
            };
        }

        public async Task<IActionResult> OnGetBindDataAsync(UserGetPageQuery query, [FromQuery] int page = 1)
        {
            query.Page = page;
            query.PageSize = _pageSize;

			PaginatedResult = await Mediator.Send(query);

			PagingInput = new PagingInput(query.Page, query.PageSize, PaginatedResult.TotalPages);

			return Partial("BindData", Tuple.Create(PaginatedResult, PagingInput));
        }
    }
}
