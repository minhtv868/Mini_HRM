//using FluentValidation;
//using Microsoft.AspNetCore.Mvc;
//using Web.Application.Features.Finance.Payrolls.Commands;
//using Web.Application.Features.Finance.Payrolls.Queries;
//using WebJob.Models;

//namespace WebJob.Pages.HRM.Payrolls
//{
//    public class EditModel : BasePageModel
//    {
//        private readonly IValidator<PayrollEditCommand> _validator;

//        public EditModel(IValidator<PayrollEditCommand> validator)
//        {
//            _validator = validator;
//        }

//        [BindProperty]
//        public new PayrollEditCommand Command { get; set; }

//        public async Task<IActionResult> OnGetAsync(int id = 0)
//        {
//            if (id <= 0)
//            {
//                return NotFound();
//            }

//            var dataGetById = await Mediator.Send(new PayrollGetByIdQuery { PayrollId = id });

//            if (dataGetById == null)
//            {
//                return NotFound();
//            }

//            Command = Mapper.Map<PayrollEditCommand>(dataGetById);
//            return Page();
//        }

//        public async Task<IActionResult> OnPostAsync()
//        {
//            var resultValidator = await _validator.ValidateAsync(Command);

//            if (!resultValidator.IsValid)
//            {
//                return new AjaxResult
//                {
//                    Succeeded = false,
//                    Messages = resultValidator.Errors.Select(x => x.ErrorMessage).ToList()
//                };
//            }

//            var updateResult = await Mediator.Send(Command);

//            return new AjaxResult
//            {
//                Id = updateResult.Data.ToString(),
//                Succeeded = updateResult.Succeeded,
//                Messages = updateResult.Messages
//            };
//        }
//    }
//}
