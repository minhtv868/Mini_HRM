using MediatR;

namespace Hangfire.MediatR
{
	public static class MediatorExtensions
	{
		public static void Enqueue(this IMediator mediator, string jobName, IRequest request, string queue = "default")
		{
			var client = new BackgroundJobClient();
			client.Enqueue<MediatorHangfireBridge>(queue, bridge => bridge.Send(jobName, request));
		}

		public static void Enqueue(this IMediator mediator, IRequest request)
		{
			var client = new BackgroundJobClient();
			client.Enqueue<MediatorHangfireBridge>(bridge => bridge.Send(request));
		}

		public static void EnqueueRecurring(this IMediator mediator, string jobName, IRequest request, string cronExpression, string queue = "default")
		{
			RecurringJob.AddOrUpdate<MediatorHangfireBridge>(jobName, queue, bridge => bridge.Send(jobName, request), cronExpression);
		}

        public static void Schedule(this IMediator mediator, string jobName, IRequest request, TimeSpan enqueueAt, string queue = "default")
        {
            var client = new BackgroundJobClient();
            client.Schedule<MediatorHangfireBridge>(queue, bridge => bridge.Send(jobName, request), enqueueAt);
        }

        public static void EnqueueRecurringDisableConcurrent(this IMediator mediator, string jobName, IRequest request, string cronExpression, string queue = "default")
		{
			RecurringJob.AddOrUpdate<MediatorHangfireBridge>(jobName, queue, bridge => bridge.SendDisableConcurrent(jobName, request), cronExpression);
		}
	}
}
