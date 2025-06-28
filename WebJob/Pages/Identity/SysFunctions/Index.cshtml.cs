using Web.Application.Features.IdentityFeatures.SysFunctions.Commands;
using Web.Application.Features.IdentityFeatures.SysFunctions.Queries;
using Web.Application.Features.IdentityFeatures.UserLogs.Queries;
using Web.Shared;
using WebJob.Helpers.Configs;
using WebJob.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebJob.Pages.Identity.SysFunctions
{
	public class IndexModel : BasePageModel
    {
        [BindProperty]
        public SysFunctionGetPageQuery Query { get; set; }

        public List<SysFunctionGetByParentDto> ParentFunctionsList { get; set; }

        [BindProperty]
        public List<int> ChkActionIds { get; set; }

        public PaginatedResult<SysFunctionGetPageDto> PaginatedResult;

        private readonly int _pageSize = AppConfig.AppSettings.PageSize;

        public List<SysFunctionGetAllDto> Data { get; set; }
        public SysFunctionGetAllDto SysFunctionGetAllDto { get; set; }
        public async Task<IActionResult> OnGetAsync(SysFunctionGetPageQuery query, [FromQuery] int page = 1)
        {
            var resultGetAllSysFunction = await Mediator.Send(new SysFunctionGetByParentQuery { HasChild = true });

            ParentFunctionsList = resultGetAllSysFunction.Data;

            Query = query;
            Query.Page = page;
            Query.PageSize = _pageSize;

            PaginatedResult = await Mediator.Send(Query);

            PagingInput = new PagingInput(Query.Page, Query.PageSize, PaginatedResult.TotalPages);

            return Page();
        }

        public async Task<IActionResult> OnPostMoveUpAsync(int id = 0, string handler = "")
        {
            if (id <= 0)
            {
                return new AjaxResult
                {
                    Succeeded = false,
                    Messages = new List<string> { $"Chức năng Id ${id} không tồn tại." }
                };
            }

            var sysFunctionMoveUpResult = await Mediator.Send(new SysFunctionMoveUpCommand { Id = id });

            return new AjaxResult
            {
                Id = id.ToString(),
                Succeeded = sysFunctionMoveUpResult.Succeeded,
                Messages = sysFunctionMoveUpResult.Messages
            };
        }

        public async Task<IActionResult> OnPostMoveDownAsync(int id = 0, string handler = "")
        {
            if (id <= 0)
            {
                return new AjaxResult
                {
                    Succeeded = false,
                    Messages = new List<string> { $"Chức năng Id ${id} không tồn tại." }
                };
            }

            var sysFunctionMoveDownResult = await Mediator.Send(new SysFunctionMoveDownCommand { Id = id });

            return new AjaxResult
            {
                Id = id.ToString(),
                Succeeded = sysFunctionMoveDownResult.Succeeded,
                Messages = sysFunctionMoveDownResult.Messages
            };
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id = 0)
        {
            if (id <= 0)
            {
                return new AjaxResult
                {
                    Succeeded = false,
                    Messages = new List<string> { $"Chức năng Id ${id} không tồn tại." }
                };
            }

            var sysFunctionDeleteResult = await Mediator.Send(new SysFunctionDeleteCommand { Id = id });

            return new AjaxResult
            {
                Id = id.ToString(),
                Succeeded = sysFunctionDeleteResult.Succeeded,
                Messages = sysFunctionDeleteResult.Messages
            };
        }

        public async Task<IActionResult> OnPostBulkActionsAsync(string chkActionIds = "")
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
                if(Query.IsEnable > 0)
                {
					await Mediator.Send(new SysFunctionSetIsEnableCommand { Id = id, IsEnable = Query.IsEnable == 1 });
				}
				else if(Query.IsShow > 0)
                {
					await Mediator.Send(new SysFunctionSetIsShowCommand { Id = id, IsShow = Query.IsShow == 1 });
				}
            }

            return new AjaxResult
            {
                Succeeded = true,
                Messages = new List<string> { "Cập nhật trạng thái chức năng thành công." }
            };
        }

        public async Task<IActionResult> OnGetBindDataAsync(SysFunctionGetPageQuery query, [FromQuery] int page = 1)
        {
            query.Page = page;
            query.PageSize = _pageSize;

            PaginatedResult = await Mediator.Send(query);

            PagingInput = new PagingInput(query.Page, query.PageSize, PaginatedResult.TotalPages);

            return Partial("BindData", Tuple.Create(PaginatedResult, PagingInput));
        }
    }
}
