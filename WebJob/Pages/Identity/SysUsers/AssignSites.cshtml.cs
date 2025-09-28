using Microsoft.AspNetCore.Mvc;
using Web.Application.Features.Finance.Sites.DTOs;
using Web.Application.Features.Finance.Sites.Queries;
using Web.Application.Features.Finance.UserSites.Queries;
using Web.Application.Features.IdentityFeatures.Users.Commands;
using WebJob.Models;

namespace WebJob.WebCMS.Pages.Identity.SysUsers
{
    public class AssignSitesModel : BasePageModel
    {
        public List<SiteGetAllDto> SiteGetAllDtos { get; set; }
        public List<int> SiteId = new List<int>();
        public int UserId { get; set; }

        public async Task<IActionResult> OnGet(int userId = 0)
        {

            if (userId == 0)
            {
                return NotFound();
            }

            var dataGetById = await Mediator.Send(new UserSiteGetByUserIdQuery() { UserId = userId });
            if (dataGetById != null && dataGetById.Any())
            {
                SiteId = dataGetById.Select(x => x.SiteId).ToList();
                // Command.DataRoleIds = dataGetById.Data.Select(x => x.DataRoleId).ToList();
            }
            UserId = userId;
            SiteGetAllDtos = await Mediator.Send(new SiteGetAllQuery());
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string chkActionIds = "", int userId = 0)
        {
            if (string.IsNullOrWhiteSpace(chkActionIds))
            {
                return new AjaxResult
                {
                    Succeeded = false,
                    Messages = new List<string> { "Vui lòng chọn quyền trước khi gán" }
                };
            }
            ;
            var selectedIds = chkActionIds?.Split(',')?.Select(short.Parse)?.ToList();
            var result = await Mediator.Send(new UserAssignSitesCommand() { UserId = userId, SelectedSiteIds = selectedIds });

            return new AjaxResult
            {
                Id = userId.ToString(),
                Succeeded = result.Succeeded,
                Messages = result.Messages
            };
        }
    }
}
