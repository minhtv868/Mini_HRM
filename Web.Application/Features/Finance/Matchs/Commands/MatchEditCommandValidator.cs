using FluentValidation;

namespace Web.Application.Features.Finance.Matchs.Commands
{
    public class MatchEditCommandValidator : AbstractValidator<MatchEditCommand>
    {
        public MatchEditCommandValidator()
        {
            //RuleFor(x => x.MessageName)
            //   .NotEmpty()
            //   .WithMessage("Tên không được để trống.")
            //   .MaximumLength(255)
            //   .WithMessage("Tên không vượt quá 255 ký tự");
            //RuleFor(x => x.SendFrom)
            //      .NotEmpty()
            //    .WithMessage("Gửi từ không được để trống.")
            //   .MaximumLength(255)
            //   .WithMessage("Gửi từ không vượt quá 255 ký tự");
            //RuleFor(x => x.Title)
            //     .NotEmpty()
            //   .WithMessage("Tiêu đề không được để trống.")
            //  .MaximumLength(255)
            //  .WithMessage("Tiêu đề không vượt quá 255 ký tự");
            //RuleFor(x => x.SendMethodId)
            //  .NotNull()
            //  .WithMessage("Cần chọn phương thức gửi")
            //  .GreaterThan((byte)0)
            //  .WithMessage("Cần chọn phương thức gửi hợp lệ");
            //RuleFor(x => x.SiteId)
            //    .NotNull()
            //    .WithMessage("Cần chọn site")
            //    .GreaterThan((short)0)
            //    .WithMessage("Cần chọn site hợp lệ");
        }
    }
}
