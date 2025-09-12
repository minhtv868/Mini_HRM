using Microsoft.AspNetCore.Mvc;
using Web.Application.Features.Finance.MessageTemplates.Commands;
using Web.Application.Features.Finance.MessageTemplates.DTOs;
using Web.Application.Features.Finance.MessageTemplates.Queries;
using Web.Shared;
using WebJob.Helpers.Configs;
using WebJob.Models;

namespace WebJob.Pages.Finance.MessageTemplates
{
    public class IndexModel : BasePageModel
    {
        [BindProperty]
        public MessageTemplateGetPageQuery Query { get; set; }
        public PaginatedResult<MessageTemplateGetPageDto> PaginatedResult;
        //public List<SiteGetAllByUserDto> SiteList;
        //public List<SendMethodGetAllDto> SendMethodList;
        private readonly int _pageSize = AppConfig.AppSettings.PageSize;
        public async Task<IActionResult> OnGet(MessageTemplateGetPageQuery query, [FromQuery] int page = 1)
        {
            Query = query;
            Query.Page = page;
            Query.PageSize = _pageSize;
            //SiteList = (await Mediator.Send(new SiteGetAllByUserQuery())).Where(x => x.SiteId > 0).ToList();
            //if (SiteList != null && SiteList.Any() && Query.SiteId == null)
            //{
            //    Query.SiteId = SiteList[0].SiteId;
            //}
            PaginatedResult = await Mediator.Send(query);
            //SendMethodList = (await Mediator.Send(new SendMethodGetAllQuery())).ToList();
            //SendMethodList.Insert(0, new SendMethodGetAllDto
            //{
            //    SendMethodId = 0,
            //    SendMethodName = "..."
            //});

            PagingInput = new PagingInput(query.Page, query.PageSize, PaginatedResult.TotalPages);
            return Page();
        }
        public async Task<IActionResult> OnGetBindDataAsync(MessageTemplateGetPageQuery query, [FromQuery] int page = 1)
        {
            query.Page = page;

            query.PageSize = _pageSize;

            PaginatedResult = await Mediator.Send(query);

            PagingInput = new PagingInput(query.Page, query.PageSize, PaginatedResult.TotalPages);
            //SiteList = (await Mediator.Send(new SiteGetAllByUserQuery())).Where(x => x.SiteId > 0).ToList();
            //SendMethodList = (await Mediator.Send(new SendMethodGetAllQuery())).ToList();
            //SendMethodList.Insert(0, new SendMethodGetAllDto
            //{
            //    SendMethodId = 0,
            //    SendMethodName = "..."
            //});
            return Partial("BindData", Tuple.Create(PaginatedResult, PagingInput, query.SiteId));
        }
        public async Task<IActionResult> OnPostBulkActionsAsync(string chkActionIds = "")
        {
            if (string.IsNullOrWhiteSpace(chkActionIds))
            {
                return new AjaxResult
                {
                    Succeeded = false,
                    Messages = new List<string> { "Vui lòng chọn MessageTemplate cần thao tác" }
                };
            }
            ;

            var selectedIds = chkActionIds?.Split(',')?.Select(int.Parse)?.ToList();

            foreach (int id in selectedIds)
            {
                await Mediator.Send(new MessageTemplateDeleteCommand { MessageTemplateId = (short)id });
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
                    Messages = new List<string> { $"MessageTemplate không t?n t?i." }
                };
            }

            var deleteResult = await Mediator.Send(new MessageTemplateDeleteCommand { MessageTemplateId = id });

            return new AjaxResult
            {
                Id = id.ToString(),
                Succeeded = deleteResult.Succeeded,
                Messages = deleteResult.Messages
            };
        }
    }
}
