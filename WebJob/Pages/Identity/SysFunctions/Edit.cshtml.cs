using AutoMapper;
using FluentValidation;
using Web.Application.Features.IdentityFeatures.Roles.Queries;
using Web.Application.Features.IdentityFeatures.SysFunctions.Commands;
using Web.Application.Features.IdentityFeatures.SysFunctions.Queries;
using WebJob.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebJob.Pages.Identity.SysFunctions
{
	public class EditModel : BasePageModel
    {
        private IValidator<SysFunctionEditCommand> _validator;

        public EditModel(IValidator<SysFunctionEditCommand> validator)
        {
            _validator = validator;
        }

        [BindProperty]
        public new SysFunctionEditCommand Command { get; set; }

        public List<RoleGetAllDto> AllRolesList;

        public IEnumerable<SysFunctionGetAllDto> AllSysFunctionsList;

        public async Task<IActionResult> OnGetAsync(int id = 0)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var functionGetById = await Mediator.Send(new SysFunctionGetByIdQuery { Id = id });

            if (functionGetById.Data == null)
            {
                return NotFound();
            }

            var allRolesList = await Mediator.Send(new RoleGetAllQuery());

            AllRolesList = allRolesList.Data;

            var sysFunctionsGetAllList = await Mediator.Send(new SysFunctionGetAllQuery());

            AllSysFunctionsList = sysFunctionsGetAllList.Data;

            Command = Mapper.Map<SysFunctionEditCommand>(functionGetById.Data);

            Command.CurrentParentId = Command.ParentItemId;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var resultValidator = await _validator.ValidateAsync(Command);

            if (!resultValidator.IsValid)
            {
                return new AjaxResult
                {
                    Succeeded = false,
                    Messages = resultValidator.Errors.Select(x => x.ErrorMessage).ToList()
                };
            }

			if (Command.Id == Command.ParentItemId)
			{
				return new AjaxResult
				{
					Succeeded = false,
					Messages = new List<string> { "Chuyên mục cha không hợp lệ." }
				};
			}

			var updateResult = await Mediator.Send(Command);

            return new AjaxResult
            {
                Succeeded = updateResult.Succeeded,
                Id = updateResult.Data.ToString(),
                Messages = updateResult.Messages
            };
        }
    }
}
