using MediatR;

namespace IC.Application.Jobs
{
    public class JobRunningStatusResetJob : IRequest
	{
	}
	internal class JobRunningStatusResetJobHandler : IRequestHandler<JobRunningStatusResetJob>
	{
		public JobRunningStatusResetJobHandler()
		{
		}
		public Task Handle(JobRunningStatusResetJob queryInput, CancellationToken cancellationToken)
		{
			JobRunningHelper.ResetRunningStatus();
			return Task.CompletedTask;
		}
	}
}
