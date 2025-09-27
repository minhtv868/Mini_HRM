using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Application.Features.Finance.Medias.Commands;
using WebJob.Models;

namespace WebJob.Pages.Finance.Medias
{
    public class CreateModel : BasePageModel
    {
        private readonly IMediator _mediator;
        //public List<SiteGetAllByUserDto> SiteList;
        //public List<SendMethodGetAllDto> SendMethodList;
        public IValidator<MediaCreateCommand> _validator;

        public CreateModel(IMediator mediator, IValidator<MediaCreateCommand> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }
        [BindProperty]
        public new MediaCreateCommand Command { get; set; }
        public async Task<IActionResult> OnGet(short siteId = 0)
        {
            Command = new MediaCreateCommand();
            //SiteList = (await Mediator.Send(new SiteGetAllByUserQuery())).Where(x => x.SiteId > 0).ToList();
            //SendMethodList = await Mediator.Send(new SendMethodGetAllQuery());
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
