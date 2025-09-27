using FluentValidation;

namespace Web.Application.Features.Finance.Departments.Commands
{
    public class DepartmentEditCommandValidator : AbstractValidator<DepartmentEditCommand>
    {
        public DepartmentEditCommandValidator()
        {
            RuleFor(x => x.DepartmentName)
               .NotEmpty()
               .WithMessage("Tên không được để trống.")
               .MaximumLength(200)
               .WithMessage("Tên không vượt quá 200 ký tự");
        }
    }
}
