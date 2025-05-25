using FluentValidation;
using IC.Application.Features.IdentityFeatures.Roles.Queries;
using IC.Application.Features.IdentityFeatures.Users.Commands;
using IC.WebJob.Models;
using Microsoft.AspNetCore.Mvc;

namespace IC.WebJob.Pages.Identity.Users
{
	public class CreateModel : BasePageModel
	{
		private IValidator<UserCreateCommand> _validator;
		public CreateModel(IValidator<UserCreateCommand> validator)
		{
			_validator = validator;
		}

		[BindProperty]
		public new UserCreateCommand Command { get; set; }

		public IEnumerable<RoleGetAllDto> AllRolesList;

		public async Task<IActionResult> OnGetAsync()
		{
			var allRolesList = await Mediator.Send(new RoleGetAllQuery());

			AllRolesList = allRolesList.Data;

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

			var userInsertResult = await Mediator.Send(Command);

			return new AjaxResult
			{
				Id = userInsertResult.Data.ToString(),
				Succeeded = userInsertResult.Succeeded,
				Messages = userInsertResult.Messages
			};
		}
	}
}
