// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using IC.Application.Features.IdentityFeatures.UserLogs.Commands;
using IC.Domain.Entities.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IC.WebJob.Areas.Identity.Pages.Account
{
	public class LogoutModel : PageModel
	{
		private readonly SignInManager<User> _signInManager;
		private readonly ILogger<LogoutModel> _logger;
		private readonly IMediator _mediator;

		public LogoutModel(SignInManager<User> signInManager, ILogger<LogoutModel> logger, IMediator mediator)
		{
			_signInManager = signInManager;
			_logger = logger;
			_mediator = mediator;
		}

		public async Task<IActionResult> OnGetAsync(string returnUrl = null)
		{
			await _mediator.Send(new UserLogCreateCommand() { UserName = HttpContext.User.Identity.Name, FromIP = HttpContext.Connection.RemoteIpAddress.ToString(), UserAction = "Đăng xuất", ActionStatus = "Thành công" });
			await _signInManager.SignOutAsync();
			_logger.LogInformation("User logged out.");
			//if (returnUrl != null)
			//{
			//    return LocalRedirect(returnUrl);
			//}
			//else
			//{
			// This needs to be a redirect so that the browser performs a new
			// request and the identity for the user gets updated.
			return Redirect("/");
			//}
		}
	}
}
