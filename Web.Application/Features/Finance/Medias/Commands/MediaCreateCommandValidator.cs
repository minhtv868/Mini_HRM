using FluentValidation;

namespace Web.Application.Features.Finance.Medias.Commands
{
    public class MediaCreateCommandValidator : AbstractValidator<MediaCreateCommand>
    {
        public MediaCreateCommandValidator()
        {
            RuleFor(x => x.MediaName)
                .NotEmpty()
                .WithMessage("Tên không được để trống.")
                .MaximumLength(255)
                .WithMessage("Tên không vượt quá 255 ký tự");
            RuleFor(x => x.FilePath)
                  .NotEmpty()
                .WithMessage("Gửi từ không được để trống.")
               .MaximumLength(255)
               .WithMessage("Gửi từ không vượt quá 255 ký tự");

            RuleFor(x => x.MediaTypeId)
              .NotNull()
              .WithMessage("Cần chọn phương thức gửi")
              .GreaterThan((byte)0)
              .WithMessage("Cần chọn phương thức gửi hợp lệ");
            RuleFor(x => x.SiteId)
                .NotNull()
                .WithMessage("Cần chọn site")
                .GreaterThan((short)0)
                .WithMessage("Cần chọn site hợp lệ");
        }
    }
}
