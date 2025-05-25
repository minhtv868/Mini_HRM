using IC.Application.Features.IdentityFeatures.Roles.Commands;
using IC.Application.Features.IdentityFeatures.Roles.Queries;
using IC.Application.Features.IdentityFeatures.SysFunctions.Queries;
using IC.WebJob.Models;
using Microsoft.AspNetCore.Mvc;

namespace IC.WebJob.Pages.Identity.SysRoles
{
	public class AssignFunctionsModel : BasePageModel
	{
		[BindProperty]
		public new RoleAssignFunctionsCommand Command { get; set; }

		public List<SysFunctionGetAllDto> Data { set; get; }

		public RoleGetByIdDto RoleGetByIdDto { get; set; }

		public async Task<IActionResult> OnGetAsync(int id = 0)
		{
            if (id <= 0)
			{
				return NotFound();
			}

			Command = new RoleAssignFunctionsCommand
			{
				Id = id
			};

			var roleGetById = await Mediator.Send(
				new RoleGetByIdQuery
				{
					Id = Command.Id
				});

			if (roleGetById.Data == null)
			{
				return NotFound();
			}

			RoleGetByIdDto = roleGetById.Data;

			var sysFunctionsGetAllList = await Mediator.Send(new SysFunctionGetAllQuery());

			Data = sysFunctionsGetAllList.Data;

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var roleAssignFunctionsResult = await Mediator.Send(Command);

			return new AjaxResult
			{
				Succeeded = roleAssignFunctionsResult.Succeeded,
				Id = roleAssignFunctionsResult.Data.ToString(),
				Messages = roleAssignFunctionsResult.Messages
			};
		}
	}
}
