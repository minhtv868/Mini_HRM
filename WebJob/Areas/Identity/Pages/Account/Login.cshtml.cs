using Web.Application.Features.IdentityFeatures.UserLogs.Commands;
using Web.Domain.Entities.Identity;
using Web.Shared;
using WebJob.Helpers.Configs;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebJob.Areas.Identity.Pages.Account
{
	public class LoginModel : PageModel
	{
		private readonly UserManager<User> _userManager;
		private readonly AppSignInManager _signInManager;
		private readonly ILogger<LoginModel> _logger;
		private readonly IMediator _mediator;

		public LoginModel(UserManager<User> userManager, AppSignInManager signInManager, ILogger<LoginModel> logger, IMediator mediator)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_logger = logger;
			_mediator = mediator;
		}

		[BindProperty]
		public InputModel Input { get; set; }

		public IList<AuthenticationScheme> ExternalLogins { get; set; }

		[BindProperty]
		public string ReturnUrl { get; set; }

		public bool Modal { get; set; }

		[TempData]
		public string ErrorMessage { get; set; }

		public class InputModel
		{
			[DisplayName("Tên truy cập/Email")]
			[Required(ErrorMessage = "{0} không để trống.")]
			public string UserNameOrEmail { get; set; }

			[DisplayName("Mật khẩu")]
			[DataType(DataType.Password)]
			[Required(ErrorMessage = "{0} không để trống.")]
			public string Password { get; set; }

			[Display(Name = "Nhớ thông tin đăng nhập?")]
			public bool RememberMe { get; set; }
		}

		public async Task OnGetAsync(string returnUrl = null, bool modal = false)
		{
			if (!string.IsNullOrEmpty(ErrorMessage))
			{
				ModelState.AddModelError(string.Empty, ErrorMessage);
			}

			returnUrl ??= Url.Content("~/");

			// Clear the existing external cookie to ensure a clean login process
			await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

			ReturnUrl = returnUrl;

			Modal = modal;
		}

		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
		{
			returnUrl ??= Url.Content("~/");

			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

			if (ModelState.IsValid)
			{
				var signInResult = await _signInManager.PasswordSignInAsync(Input.UserNameOrEmail, Input.Password, Input.RememberMe, lockoutOnFailure: false);

				if (!signInResult.Succeeded)
				{
					var user = await _userManager.FindByEmailAsync(Input.UserNameOrEmail);

					if (user != null)
					{
						signInResult = await _signInManager.PasswordSignInAsync(
							user,
							Input.Password,
							Input.RememberMe,
							lockoutOnFailure: false
						);
					}
				}

				if (signInResult.Succeeded)
				{
					_logger.LogInformation("User logged in.");

					await _mediator.Send(new UserLogCreateCommand() { UserName = Input.UserNameOrEmail, FromIP = HttpContext.Connection.RemoteIpAddress.ToString(), UserAction = "Đăng nhập", ActionStatus = "Thành công" });

					return new JsonResult(new
					{
						Succeeded = true,
						ReturnUrl = returnUrl
					});
				}

				if (signInResult.RequiresTwoFactor)
				{
					await _mediator.Send(new UserLogCreateCommand() { UserName = Input.UserNameOrEmail, FromIP = HttpContext.Connection.RemoteIpAddress.ToString(), UserAction = "Đăng nhập", ActionStatus = "Vào trang 2FA" });

                    return new JsonResult(new
					{
						Succeeded = false,
						ReturnUrl = Url.Page("/Account/LoginWith2fa", null, new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe })
					});
                }

				if (signInResult.IsLockedOut)
				{
					_logger.LogWarning("User account locked out.");

					await _mediator.Send(new UserLogCreateCommand() { UserName = Input.UserNameOrEmail, FromIP = HttpContext.Connection.RemoteIpAddress.ToString(), UserAction = "Đăng nhập", ActionStatus = "Tài khoản bị khóa" });

					return new JsonResult(new
					{
						Succeeded = false,
						Messages = $"Tài khoản <b>{Input.UserNameOrEmail}</b> đã bị khóa, vui lòng thử lại sau."
					});
				}
				else
				{
					await _mediator.Send(new UserLogCreateCommand() { UserName = Input.UserNameOrEmail, FromIP = HttpContext.Connection.RemoteIpAddress.ToString(), UserAction = "Đăng nhập", ActionStatus = "Không thành công" });

					return new JsonResult(new
					{
						Succeeded = false,
						Messages = "Thông tin đăng nhập không chính xác."
					});
				}
			}

			return new JsonResult(new
			{
				Succeeded = false,
				Messages = AppConfig.AppSettings.ErrorMessage
			});
		}
	}
}
