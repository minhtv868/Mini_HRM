using FluentValidation;
using IC.Application.Features.IdentityFeatures.Roles.Commands;
using IC.WebJob.Models;
using Microsoft.AspNetCore.Mvc;

namespace IC.WebJob.Pages.Identity.SysRoles
{
	public class CreateModel : BasePageModel
	{
		private IValidator<RoleCreateCommand> _validator;

		[BindProperty]
		public new RoleCreateCommand Command { get; set; }

		public CreateModel(IValidator<RoleCreateCommand> validator)
		{
			_validator = validator;
		}

		public IActionResult OnGet()
		{
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

			var roleInsertResult = await Mediator.Send(Command);

			return new AjaxResult
			{
				Id = roleInsertResult.Data.ToString(),
				Succeeded = roleInsertResult.Succeeded,
				Messages = roleInsertResult.Messages
			};
		}
	}
}
