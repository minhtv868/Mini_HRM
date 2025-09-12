using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Web.Application.Features.Finance.MessageTemplates.Commands;
using Web.Application.Features.Finance.MessageTemplates.Queries;
using WebJob.Models;

namespace WebJob.Pages.Finance.MessageTemplates
{
    public class EditModel : BasePageModel
    {
        private readonly IMediator _mediator;
        //public List<SiteGetAllByUserDto> SiteList;
        //public List<SendMethodGetAllDto> SendMethodList;
        public IValidator<MessageTemplateEditCommand> _validator;

        public EditModel(IMediator mediator, IValidator<MessageTemplateEditCommand> validator)
        {
            _mediator = mediator;
            _validator = validator;
        }
        [BindProperty]
        public new MessageTemplateEditCommand Command { get; set; }
        public async Task<IActionResult> OnGet(short id = 0)
        {
            if (id == 0)
            {
                return NotFound();
            }
            var dataGetById = await Mediator.Send(new MessageTemplateGetByIdQuery { MessageTemplateId = id });

            if (dataGetById.Data == null)
            {
                return NotFound();
            }
            //SiteList = (await Mediator.Send(new SiteGetAllByUserQuery())).Where(x => x.SiteId > 0).ToList();
            //SendMethodList = await Mediator.Send(new SendMethodGetAllQuery());
            Command = Mapper.Map<MessageTemplateEditCommand>(dataGetById.Data);
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
