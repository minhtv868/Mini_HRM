using IC.Application.Features.IdentityFeatures.Roles.Commands;
using IC.Application.Features.IdentityFeatures.Roles.Queries;
using IC.WebJob.Helpers.Configs;
using IC.WebJob.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IC.WebJob.Pages.Identity.SysRoles
{
	public class IndexModel : BasePageModel
	{
		[BindProperty]
		public RoleGetAllQuery Query { get; set; }

		public List<RoleGetAllDto> Data { set; get; }

		public async Task<IActionResult> OnGetAsync(RoleGetAllQuery query)
		{
			var rolesGetAll = await Mediator.Send(query);

			Data = rolesGetAll.Data;

			return Page();
		}

		public async Task<IActionResult> OnPostDeleteAsync(int id = 0)
		{
            if (id <= 0)
			{
				return new AjaxResult
				{
					Succeeded = false,
					Messages = new List<string> { $"Vai trò Id ${id} không tồn tại." }
				};
			}

			var roleDeleteResult = await Mediator.Send(new RoleDeleteCommand { Id = id });

			return new AjaxResult
			{
				Id = id.ToString(),
				Succeeded = roleDeleteResult.Succeeded,
				Messages = roleDeleteResult.Messages
			};
		}

		public async Task<IActionResult> OnGetBindDataAsync(RoleGetAllQuery query)
		{
			var rolesGetAll = await Mediator.Send(query);

			return Partial("BindData", rolesGetAll.Data);
		}
	}
}
