using FluentValidation;
using IC.Application.Features.IdentityFeatures.Roles.Commands;
using IC.Application.Features.IdentityFeatures.Roles.Queries;
using IC.WebJob.Models;
using Microsoft.AspNetCore.Mvc;

namespace IC.WebJob.Pages.Identity.SysRoles
{
    public class EditModel : BasePageModel
    {
        private IValidator<RoleEditCommand> _validator;

        public EditModel(IValidator<RoleEditCommand> validator)
        {
            _validator = validator;
        }

        [BindProperty]
        public new RoleEditCommand Command { get; set; }

        public async Task<IActionResult> OnGetAsync(int id = 0)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var roleGetById = await Mediator.Send(new RoleGetByIdQuery { Id = id });

            if (roleGetById.Data == null)
            {
                return NotFound();
            }

            Command = Mapper.Map<RoleEditCommand>(roleGetById.Data);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var resultValidator = await _validator.ValidateAsync(Command);

            if (!resultValidator.IsValid)
            {
                return new AjaxResult
                {
                    Succeeded = false,
                    Messages = resultValidator.Errors.Select(x => x.ErrorMessage).ToList()
                };
            }

            var updateResult = await Mediator.Send(Command);

            return new AjaxResult
            {
                Id = updateResult.Data.ToString(),
                Succeeded = updateResult.Succeeded,
                Messages = updateResult.Messages
            };
        }
    }
}

