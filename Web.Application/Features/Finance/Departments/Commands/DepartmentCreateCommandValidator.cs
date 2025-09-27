using FluentValidation;

namespace Web.Application.Features.Finance.Departments.Commands
{
    public class DepartmentCreateCommandValidator : AbstractValidator<DepartmentCreateCommand>
    {
        public DepartmentCreateCommandValidator()
        {
            RuleFor(x => x.DepartmentName)
                .NotEmpty()
                .WithMessage("Tên không được để trống.")
                .MaximumLength(200)
                .WithMessage("Tên không vượt quá 200 ký tự");

            RuleFor(x => x.SiteId)
                .NotNull()
                .WithMessage("Cần chọn site")
                .GreaterThan(0)
                .WithMessage("Cần chọn site hợp lệ");
        }
    }
}
