using FluentValidation;
using IC.Application.Features.IdentityFeatures.Roles.Queries;
using IC.Application.Features.IdentityFeatures.SysFunctions.Commands;
using IC.Application.Features.IdentityFeatures.SysFunctions.Queries;
using IC.WebJob.Models;
using Microsoft.AspNetCore.Mvc;

namespace IC.WebJob.Pages.Identity.SysFunctions
{
	public class CreateModel : BasePageModel
	{
		private IValidator<SysFunctionCreateCommand> _validator;
		public CreateModel(IValidator<SysFunctionCreateCommand> validator)
		{
			_validator = validator;
		}

		[BindProperty]
		public new SysFunctionCreateCommand Command { get; set; }

		public IEnumerable<RoleGetAllDto> AllRolesList;

		public IEnumerable<SysFunctionGetAllDto> AllSysFunctionsList;

		public async Task<IActionResult> OnGet()
		{
            var allRolesList = await Mediator.Send(new RoleGetAllQuery());

			AllRolesList = allRolesList.Data;

			var sysFunctionsGetAllList = await Mediator.Send(new SysFunctionGetAllQuery());

			AllSysFunctionsList = sysFunctionsGetAllList.Data;

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

			var functionInsertResult = await Mediator.Send(Command);

			return new AjaxResult
			{
				Id = functionInsertResult.Data.ToString(),
				Succeeded = functionInsertResult.Succeeded,
				Messages = functionInsertResult.Messages
			};
		}
	}
}
