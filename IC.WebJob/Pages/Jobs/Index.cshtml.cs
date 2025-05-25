using Hangfire.MediatR;
using Hangfire.Storage;
using IC.Application.Features.BongDa24hJobs.Jobs.DTOs;
using IC.Application.Features.BongDa24hJobs.Jobs.Queries;
using IC.Application.Jobs;
using IC.WebJob.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace IC.WebJob.Pages.Jobs
{

	public class IndexModel : BasePageModel
    {
		public string Message { get; set; }
		private readonly IMediator _mediator;

		[BindProperty]
		[DisplayName("Tên hàng đợi")]
		public string JobQueue { get; set; }

		[BindProperty]
		[DisplayName("Công việc")]
		public int JobId { get; set; }

		[BindProperty]
		[DisplayName("Lịch thực hiện")]
		public string JobScheduleType { get; set; }

		[BindProperty]

		[DisplayName("Với khoảng thời gian")]
		public int JobScheduleTimeNumber { get; set; } = 5;
        [BindProperty]
        public List<RecurringJobDto> RecurringJobs { get; set; }
        public IEnumerable<JobGetAllDto> JobList { get; set; }

        public IndexModel(IMediator mediator)
		{
			_mediator = mediator;
		}
		public async Task<IActionResult> OnGetAsync()
        {
            RecurringJobs = Hangfire.JobStorage.Current.GetConnection().GetRecurringJobs();

            JobList = await Mediator.Send(new JobGetAllQuery());


			return Page();
		}
		public async Task<IActionResult> OnPostAsync()
		{  
			var jobData =await Mediator.Send(new JobGetByIdQuery() { Id = JobId });

			if (jobData.Succeeded && jobData.Data != null)
			{
				try
				{

				var jobItem = jobData.Data;

				Type t = Type.GetType(jobItem.JobClassType);

				IRequest job = (IRequest)Activator.CreateInstance(t);

				var jobScheduleTime = IcJobHelper.GetJobSchedule(JobScheduleType, JobScheduleTimeNumber);

				if (jobScheduleTime == "")
				{
					_mediator.Enqueue(jobItem.JobClassName, job, JobQueue);
				}
				else
				{
					string jobFullName = string.Format("[{0}] - {1} - {2} - {3}", JobQueue.ToUpper(), jobItem.JobName, JobScheduleType, JobScheduleTimeNumber);
					if (JobScheduleType == "Chạy 1 lần")
					{
						jobFullName = string.Format("[{0}] - {1} - {2}", JobQueue.ToUpper(), jobItem.JobName, JobScheduleType);
					}
					_mediator.EnqueueRecurring(jobFullName, job, jobScheduleTime, JobQueue);
				} 
				Message = jobItem.JobName + " đã được thêm vào hàng đợi";
				return new AjaxResult()
				{
					Succeeded = true,
					Messages = new List<string>() { Message }
				};
                }
                catch (Exception ex) {
                    return new AjaxResult()
                    {
                        Succeeded = false,
                        Messages = new List<string>() { ex.ToString() }
                    };
                }
            }
			return new AjaxResult()
			{
				Succeeded = false,
				Messages = new List<string>() { "Không tìm thấy thông tin Job" }
			};
		}
	}
}
