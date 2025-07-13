using Web.Application.Features.Finances.Jobs.Commands;
using Web.Application.Features.Finances.Jobs.Queries;
using WebJob.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebJob.Pages.WebJobs.Jobs
{
    public class EditModel : BasePageModel
    {

        [BindProperty]
        public new JobEditCommand Command { get; set; }

        public async Task<IActionResult> OnGetAsync(int id = 0)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var dataGetById = await Mediator.Send(new JobGetByIdQuery { Id = id });

            if (dataGetById.Data == null)
            {
                return NotFound();
            }

            Command = Mapper.Map<JobEditCommand>(dataGetById.Data);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var updateResult = await Mediator.Send(Command);

            return new AjaxResult
            {
                Succeeded = updateResult.Succeeded,
                Id = updateResult.Data.ToString(),
                Messages = updateResult.Messages
            };
        }
    }
}
