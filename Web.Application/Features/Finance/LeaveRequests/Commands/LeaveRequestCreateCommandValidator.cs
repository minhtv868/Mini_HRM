using FluentValidation;

namespace Web.Application.Features.Finance.LeaveRequests.Commands
{
    public class LeaveRequestCreateCommandValidator : AbstractValidator<LeaveRequestCreateCommand>
    {
        public LeaveRequestCreateCommandValidator()
        {


            RuleFor(x => x.SiteId)
                .NotNull()
                .WithMessage("Cần chọn site")
                .GreaterThan(0)
                .WithMessage("Cần chọn site hợp lệ");
        }
    }
}
