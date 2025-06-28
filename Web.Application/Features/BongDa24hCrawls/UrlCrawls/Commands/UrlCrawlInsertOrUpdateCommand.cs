using AutoMapper;
using Web.Application.Interfaces.Repositories.BongDa24hCrawls;
using Web.Domain.Entities.Crawls;
using Web.Shared.Helpers;
using Web.Shared;
using MediatR;
using Web.Application.Common.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Web.Application.Features.BongDa24hCrawls.UrlCrawls.Commands
{
	public record UrlCrawlInsertOrUpdateCommand : IRequest<Result<int>>, IMapFrom<UrlCrawl>
	{
		public string Url { get; set; }
		public string UrlDesc { get; set; }
		public string UrlType { get; set; }
		public long UrlHash { get; set; }
		public string UrlGroup { get; set; }
		public bool IsCrawled { get; set; } = false;

		public bool AutoCreateListPage { get; set; } = false;
		public int MaxPage { get; set; } = 0;
	}

	internal class UrlCrawlInsertOrUpdateCommandHandler : IRequestHandler<UrlCrawlInsertOrUpdateCommand, Result<int>>
	{
		private readonly IBongDa24HCrawlUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
		private readonly IMediator _mediator;

		public UrlCrawlInsertOrUpdateCommandHandler(IBongDa24HCrawlUnitOfWork unitOfWork, IMapper mapper, IMediator mediator)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
			_mediator = mediator;
		}

		public async Task<Result<int>> Handle(UrlCrawlInsertOrUpdateCommand command, CancellationToken cancellationToken)
		{ 
			if (command.UrlHash == 0)
			{
				command.UrlHash = StringHelper.CreateId(command.Url, true, System.Text.Encoding.UTF8);
			}
			var urlCrawl = await _unitOfWork.Repository<UrlCrawl>().Entities.AsNoTracking().Where(x => x.UrlHash == command.UrlHash).FirstOrDefaultAsync(cancellationToken);

			if (urlCrawl != null)
			{
				urlCrawl.IsCrawled = command.IsCrawled;

				urlCrawl.BatchCode = null;

				await _unitOfWork.Repository<UrlCrawl>().UpdateFieldsAsync(urlCrawl, x => x.IsCrawled, x => x.BatchCode);

				var updateResult  = await _unitOfWork.Save(cancellationToken);

				if (updateResult > 0)
				{
					return await Result<int>.FailureAsync("Cập nhật thành công");
				}

				return await Result<int>.FailureAsync("Cập nhật thất bại");
			}


			var commandCreate = _mapper.Map<UrlCrawlCreateCommand>(command);

			return await _mediator.Send(commandCreate);
		}
	}
}
