using Web.Application.Features.IdentityFeatures.Users.Commands;
using Web.Application.Features.IdentityFeatures.Users.Queries;
using WebJob.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebJob.Pages.Identity.SysUsers
{
	public class ResetPasswordModel : BasePageModel
	{
		[BindProperty]
		public new UserResetPasswordCommand Command { get; set; }

		public UserGetByIdDto UserGetByIdDto { get; set; }

		public async Task<IActionResult> OnGetAsync(int id = 0)
		{
            if (id <= 0)
			{
				return NotFound();
			}

			Command = new UserResetPasswordCommand
			{
				Id = id
			};

			var userGetById = await Mediator.Send(new UserGetByIdQuery { Id = Command.Id });

			if (userGetById.Data == null)
			{
				return NotFound();
			}

			UserGetByIdDto = userGetById.Data;

			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var userResetPasswordResult = await Mediator.Send(Command);

			return new AjaxResult
			{
				Succeeded = userResetPasswordResult.Succeeded,
				Id = userResetPasswordResult.Data.ToString(),
				Messages = userResetPasswordResult.Messages
			};
		}
	}
}
