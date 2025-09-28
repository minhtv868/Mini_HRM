using FluentValidation;

namespace Web.Application.Features.Finance.Attendances.Commands
{
    public class AttendanceCreateCommandValidator : AbstractValidator<AttendanceCreateCommand>
    {
        public AttendanceCreateCommandValidator()
        {


            RuleFor(x => x.SiteId)
                .NotNull()
                .WithMessage("Cần chọn site")
                .GreaterThan(0)
                .WithMessage("Cần chọn site hợp lệ");
        }
    }
}
