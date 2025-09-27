using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Web.Application.Features.Finance.Departments.Commands;
using Web.Application.Features.Finance.Sites.DTOs;
using Web.Application.Features.Finance.Sites.Queries;
using WebJob.Models;

namespace WebJob.Pages.Finance.Departments
{
    public class CreateModel : BasePageModel
    {
        private readonly IValidator<DepartmentCreateCommand> _validator;
        public List<SiteGetAllByUserDto> SiteList;
        public CreateModel(IValidator<DepartmentCreateCommand> validator)
        {
            _validator = validator;
        }

        [BindProperty]
        public new DepartmentCreateCommand Command { get; set; }
        public async Task<IActionResult> OnGet(int siteId)
        {
            Command = new DepartmentCreateCommand();
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
