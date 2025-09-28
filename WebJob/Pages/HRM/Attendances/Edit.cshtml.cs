using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Web.Application.Features.Finance.Attendances.Commands;
using Web.Application.Features.Finance.Attendances.Queries;
using WebJob.Models;

namespace WebJob.Pages.HRM.Attendances
{
    public class EditModel : BasePageModel
    {
        private readonly IValidator<AttendanceEditCommand> _validator;

        public EditModel(IValidator<AttendanceEditCommand> validator)
        {
            _validator = validator;
        }

        [BindProperty]
        public new AttendanceEditCommand Command { get; set; }

        public async Task<IActionResult> OnGetAsync(int id = 0)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var dataGetById = await Mediator.Send(new AttendanceGetByIdQuery { AttendanceId = id });

            if (dataGetById == null)
            {
                return NotFound();
            }

            Command = Mapper.Map<AttendanceEditCommand>(dataGetById);
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
