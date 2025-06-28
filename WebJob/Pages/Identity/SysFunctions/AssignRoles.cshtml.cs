using Web.Application.Features.IdentityFeatures.Roles.Queries;
using Web.Application.Features.IdentityFeatures.SysFunctions.Commands;
using Web.Application.Features.IdentityFeatures.SysFunctions.Queries;
using WebJob.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebJob.Pages.Identity.SysFunctions
{
	public class AssignRolesModel : BasePageModel
    {
        [BindProperty]
        public new SysFunctionAssignRolesCommand Command { get; set; }

        public List<RoleGetAllDto> Data { set; get; }

        public SysFunctionGetByIdDto SysFunctionGetByIdDto { get; set; }

        public async Task<IActionResult> OnGetAsync(int id = 0)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var sysFunctionGetById = await Mediator.Send(new SysFunctionGetByIdQuery { Id = id });

            if (sysFunctionGetById.Data == null)
            {
                return NotFound();
            }

			Command = new SysFunctionAssignRolesCommand
			{
				Id = id
			};

			SysFunctionGetByIdDto = sysFunctionGetById.Data;

            var rolesGetListAll = await Mediator.Send(new RoleGetAllQuery());

            Data = rolesGetListAll.Data;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var sysFunctionAssignRolesResult = await Mediator.Send(Command);

            return new AjaxResult
            {
                Succeeded = sysFunctionAssignRolesResult.Succeeded,
                Id = sysFunctionAssignRolesResult.Data.ToString(),
                Messages = sysFunctionAssignRolesResult.Messages
            };
        }
    }
}
