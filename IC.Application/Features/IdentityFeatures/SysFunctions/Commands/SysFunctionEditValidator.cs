using FluentValidation;

namespace IC.Application.Features.IdentityFeatures.SysFunctions.Commands
{
    public class SysFunctionEditValidator : AbstractValidator<SysFunctionEditCommand>
    {
        public SysFunctionEditValidator()
        {
            RuleFor(x => x.FunctionName)
                .NotEmpty() 
                .WithMessage("Tên chức năng không để trống.");
        }
    }
}
