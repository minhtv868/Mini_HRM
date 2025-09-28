using FluentValidation;

namespace Web.Application.Features.Finance.Attendances.Commands
{
    public class AttendanceEditCommandValidator : AbstractValidator<AttendanceEditCommand>
    {
        public AttendanceEditCommandValidator()
        {
            RuleFor(x => x.AttendanceName)
               .NotEmpty()
               .WithMessage("Tên không được để trống.")
               .MaximumLength(200)
               .WithMessage("Tên không vượt quá 200 ký tự");
        }
    }
}