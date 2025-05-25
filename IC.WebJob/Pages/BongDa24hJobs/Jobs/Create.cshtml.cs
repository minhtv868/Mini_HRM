using IC.Application.Features.BongDa24hJobs.Jobs.Commands;
using IC.WebJob.Models;
using Microsoft.AspNetCore.Mvc;

namespace IC.WebJob.Pages.BongDa24hJobs.Jobs
{
	public class CreateModel : BasePageModel
	{  
		[BindProperty]
		public new JobCreateCommand Command { get; set; }

		public IActionResult OnGet()
		{
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{ 
			var dataInsertResult = await Mediator.Send(Command);

			return new AjaxResult
			{
				Id = dataInsertResult.Data.ToString(),
				Succeeded = dataInsertResult.Succeeded,
				Messages = dataInsertResult.Messages
			};
		}
	}
}
