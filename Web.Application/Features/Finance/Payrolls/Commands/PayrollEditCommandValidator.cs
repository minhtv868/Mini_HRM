using FluentValidation;

namespace Web.Application.Features.Finance.Payrolls.Commands
{
    public class PayrollEditCommandValidator : AbstractValidator<PayrollEditCommand>
    {
        public PayrollEditCommandValidator()
        {
            RuleFor(x => x.UserId)
                   .NotNull()
                   .WithMessage("Cần chọn site")
                   .GreaterThan(0)
                   .WithMessage("Cần chọn site hợp lệ");
            RuleFor(x => x.SiteId)
                .NotNull()
                .WithMessage("Cần chọn site")
                .GreaterThan(0)
                .WithMessage("Cần chọn site hợp lệ");
        }
    }
}