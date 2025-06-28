using Web.Application.Interfaces.Repositories.BongDa24hCrawls;
using Web.Domain.Entities.Crawls;
using MediatR;

namespace Web.Application.Features.BongDa24hCrawls.UrlCrawls.Commands
{
	public record UrlCrawlUpdateResultCommand : IRequest
	{
		public int Id { get; set; }
		public int JobId { get; set; }
		public string CrawlResult { get; set; }
	}

	internal class UrlCrawlUpdateResultCommandHandler : IRequestHandler<UrlCrawlUpdateResultCommand>
	{
		private readonly IBongDa24HCrawlUnitOfWork _unitOfWork;

		public UrlCrawlUpdateResultCommandHandler(IBongDa24HCrawlUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public async Task Handle(UrlCrawlUpdateResultCommand command, CancellationToken cancellationToken)
		{
			var entity = new UrlCrawl()
			{
				Id = command.Id,
				CrawlResult = command.CrawlResult,
				IsCrawled = true,
				CrawlTime = DateTime.Now
			};

			if (!string.IsNullOrEmpty(entity.CrawlResult) && entity.CrawlResult.Length > 250)
			{
				entity.CrawlResult = entity.CrawlResult.Substring(0, 250);
			}

			await _unitOfWork.Repository<UrlCrawl>().UpdateFieldsAsync(entity,
																	   x => x.CrawlResult,
																	   x => x.IsCrawled,
																	   x => x.CrawlTime); 
			await _unitOfWork.Save(cancellationToken);
		}
	}
}
