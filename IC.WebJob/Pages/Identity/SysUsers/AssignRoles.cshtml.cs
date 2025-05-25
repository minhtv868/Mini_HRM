using IC.Application.Features.IdentityFeatures.Roles.Queries;
using IC.Application.Features.IdentityFeatures.Users.Commands;
using IC.Application.Features.IdentityFeatures.Users.Queries;
using IC.WebJob.Models;
using Microsoft.AspNetCore.Mvc;

namespace IC.WebJob.Pages.Identity.SysUsers
{
	public class AssignRolesModel : BasePageModel
	{
		[BindProperty]
		public new UserAssignRolesCommand Command { get; set; }

		public List<RoleGetAllDto> Data { set; get; }

		public UserGetByIdDto UserGetByIdDto { get; set; }

		public async Task<IActionResult> OnGetAsync(int id = 0)
		{
            if (id <= 0)
			{
				return NotFound();
			}

			Command = new UserAssignRolesCommand
			{
				Id = id
			};

			var userGetById = await Mediator.Send(new UserGetByIdQuery { Id = Command.Id });

			if (userGetById.Data == null)
			{
				return NotFound();
			}

			UserGetByIdDto = userGetById.Data;

			var rolesGetListAll = await Mediator.Send(new RoleGetAllQuery());

			Data = rolesGetListAll.Data;

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var userAssignRolesResult = await Mediator.Send(Command);

			return new AjaxResult
			{
				Succeeded = userAssignRolesResult.Succeeded,
				Id = userAssignRolesResult.Data.ToString(),
				Messages = userAssignRolesResult.Messages
			};
		}
	}
}
