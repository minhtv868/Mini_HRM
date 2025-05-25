using AutoMapper;
using FluentValidation;
using IC.Application.Features.IdentityFeatures.Roles.Queries;
using IC.Application.Features.IdentityFeatures.Users.Commands;
using IC.Application.Features.IdentityFeatures.Users.Queries;
using IC.WebJob.Models;
using Microsoft.AspNetCore.Mvc;

namespace IC.WebJob.Pages.Identity.Users
{
	public class EditModel : BasePageModel
	{
		private IValidator<UserEditCommand> _validator;

		public EditModel(IValidator<UserEditCommand> validator)
		{
			_validator = validator;
		}

		[BindProperty]
		public new UserEditCommand Command { get; set; }

		public List<RoleGetAllDto> AllRolesList;

		public async Task<IActionResult> OnGetAsync(int id = 0)
		{
            if (id <= 0)
			{
				return NotFound();
			}

			var userGetById = await Mediator.Send(new UserGetByIdQuery { Id = id });

			if (userGetById.Data == null)
			{
				return NotFound();
			}

			var allRolesList = await Mediator.Send(new RoleGetAllQuery());

			AllRolesList = allRolesList.Data;

			Command = Mapper.Map<UserEditCommand>(userGetById.Data);

			if (userGetById.Data.BirthDay.HasValue && userGetById.Data.BirthDay.Value != DateTime.MinValue)
			{
				Command.BirthDay = userGetById.Data.BirthDay.Value.ToString("dd-MM-yyyy");
			}

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

			var updateResult = await Mediator.Send(Command);

			return new AjaxResult
			{
				Id = updateResult.Data.ToString(),
				Succeeded = updateResult.Succeeded,
				Messages = updateResult.Messages
			};
		}
    }
}
