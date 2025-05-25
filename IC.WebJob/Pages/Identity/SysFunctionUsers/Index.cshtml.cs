using IC.Application.Features.IdentityFeatures.SysFunctions.Queries;
using IC.Application.Features.IdentityFeatures.SysFunctionUsers.Commands;
using IC.WebJob.Models;
using Microsoft.AspNetCore.Mvc;

namespace IC.WebJob.Pages.Identity.SysFunctionUsers
{
    public class IndexModel : BasePageModel
    {
        public List<SysFunctionGetMenuByUserDto> SysFunctionGetMenuByUsersList { get; set; }
		public async Task<IActionResult> OnGetAsync()
        {
			var sysFunctionsListByUser = await Mediator.Send(new SysFunctionGetMenuByUserQuery(CurrentUserService.UserId, true));

            if(sysFunctionsListByUser.Data != null && sysFunctionsListByUser.Data.Any())
            {
                SysFunctionGetMenuByUsersList = sysFunctionsListByUser.Data.FindAll(x => x.IsFavorite)
                    .OrderBy(x => x.DisplayOrder).ToList();
			}

			return Page();
        }

        public async Task<IActionResult> OnPostUpdateOrderAsync(List<SysFunctionUsersUpdateDisplayOrderCommand> commands)
        {
            foreach (var command in commands)
            {
                await Mediator.Send(new SysFunctionUsersUpdateDisplayOrderCommand { UserId = CurrentUserService.UserId, Id = command.Id, DisplayOrder = command.DisplayOrder });
            }

            return new AjaxResult
            {
                Succeeded = true,
                Messages = new List<string> { "Cập nhật vị trí chức năng ưa thích thành công." }
            };
        }
    }
}
