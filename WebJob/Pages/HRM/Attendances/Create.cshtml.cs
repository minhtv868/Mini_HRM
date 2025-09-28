using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Web.Application.Features.Finance.Attendances.Commands;
using Web.Application.Features.Finance.Sites.DTOs;
using Web.Application.Features.Finance.Sites.Queries;
using WebJob.Models;

namespace WebJob.Pages.HRM.Attendances
{
    public class CreateModel : BasePageModel
    {
        private readonly IValidator<AttendanceCreateCommand> _validator;
        public List<SiteGetAllByUserDto> SiteList;
        public CreateModel(IValidator<AttendanceCreateCommand> validator)
        {
            _validator = validator;
        }

        [BindProperty]
        public new AttendanceCreateCommand Command { get; set; }
        public async Task<IActionResult> OnGet(int siteId)
        {
            Command = new AttendanceCreateCommand();
            SiteList = await Mediator.Send(new SiteGetAllByUserQuery());
            Command.SiteId = siteId;
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

            var insertResult = await Mediator.Send(Command);

            return new AjaxResult
            {
                Id = insertResult.Data.ToString(),
                Succeeded = insertResult.Succeeded,
                Messages = insertResult.Messages
            };
        }
    }
}
