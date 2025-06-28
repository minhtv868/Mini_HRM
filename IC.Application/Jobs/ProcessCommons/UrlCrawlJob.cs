using MediatR;
using Microsoft.Extensions.Logging;

namespace IC.Application.Jobs.ProcessCommons
{
    public record UrlCrawlJob : IRequest
    {

    }
    internal class UrlCrawlJobHandler : IRequestHandler<UrlCrawlJob>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<UrlCrawlJobHandler> _logger;

        public UrlCrawlJobHandler(IMediator mediator, ILogger<UrlCrawlJobHandler> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Handle(UrlCrawlJob command, CancellationToken cancellationToken)
        {
            //Lấy ds crawl job
            //var crawlJobs = await _mediator.Send(new UrlCrawlJobGetAllQuery());

            //if (crawlJobs != null && crawlJobs.Any())
            //{
            //	foreach (var crawlJob in crawlJobs)
            //	{
            //		if (string.IsNullOrEmpty(crawlJob.ProcessClassType)) continue;

            //		try
            //		{
            //			//Khởi tạo job động và đưa vào hangfire queue
            //			Type t = Type.GetType(crawlJob.ProcessClassType);
            //			IRequest job = (IRequest)Activator.CreateInstance(t, crawlJob);
            //			var client = new BackgroundJobClient();
            //			client.Enqueue<CrawlDataHangfireBridge>(crawlJob.JobQueue, bridge => bridge.Send(crawlJob.JobName, job));
            //		}
            //		catch (Exception ex)
            //		{
            //			_logger.LogError(ex.Message, ex);
            //		}

            //		//cập nhật next time run
            //		await _mediator.Send(new UrlCrawlJobUpdateNextTimeCommand() { UrlCrawlJob = crawlJob });
            //	}
            //}
            int x = 2;
        }
    }
}
