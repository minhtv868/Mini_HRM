using FluentValidation;

namespace IC.Application.Features.IdentityFeatures.SysFunctions.Commands
{
    public class SysFunctionCreateValidator : AbstractValidator<SysFunctionCreateCommand>
    {
        public SysFunctionCreateValidator()
        {
            RuleFor(x => x.FunctionName)
                .NotEmpty() 
                .WithMessage("Tên chức năng không để trống.");
        }
    }
}
