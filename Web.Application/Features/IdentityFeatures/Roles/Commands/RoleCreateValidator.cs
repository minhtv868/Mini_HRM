using FluentValidation;

namespace Web.Application.Features.IdentityFeatures.Roles.Commands
{
	public class RoleCreateValidator : AbstractValidator<RoleCreateCommand>
	{
		public RoleCreateValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty()
				.WithMessage("Tên không để trống.");

			RuleFor(x => x.Description)
				.NotEmpty()
				.WithMessage("Mô tả không để trống.");
		}
	}
}
