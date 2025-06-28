using AutoMapper;
using Web.Application.Common.Mappings;
using Web.Domain.Entities.Crawls;
using Web.Shared.Helpers;
using Web.Shared;
using MediatR;
using Web.Application.Interfaces.Repositories.BongDa24hCrawls;
using Microsoft.EntityFrameworkCore;

namespace Web.Application.Features.BongDa24hCrawls.UrlCrawls.Commands
{
	public record UrlCrawlCreateCommand : IRequest<Result<int>>, IMapFrom<UrlCrawl>, IMapFrom<UrlCrawlInsertOrUpdateCommand>
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

	internal class UrlCrawlCreateCommandHandler : IRequestHandler<UrlCrawlCreateCommand, Result<int>>
	{
		private readonly IBongDa24HCrawlUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;

		public UrlCrawlCreateCommandHandler(IBongDa24HCrawlUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}

		public async Task<Result<int>> Handle(UrlCrawlCreateCommand command, CancellationToken cancellationToken)
		{
			if (command.AutoCreateListPage)
			{
				var count = await _unitOfWork.Repository<UrlCrawl>().Entities
					.Where(x => x.UrlGroup == command.UrlGroup && x.UrlType == "ListPage")
					.CountAsync(cancellationToken);

				if (count <= command.MaxPage)
				{
					count++;
					command.Url = command.Url.Replace("{page}", count.ToString());
					command.UrlDesc = "Page " + count.ToString();
				}
			}

			if (command.UrlHash == 0)
			{
				command.UrlHash = StringHelper.CreateId(command.Url, true, System.Text.Encoding.UTF8);
			}

			if (_unitOfWork.Repository<UrlCrawl>().Entities.Any(x => x.UrlHash == command.UrlHash))
			{
				return await Result<int>.FailureAsync(0, "Url này đã có");
			}

			var entity = _mapper.Map<UrlCrawl>(command);

			entity.CrDateTime = DateTime.Now;

			if (!string.IsNullOrEmpty(entity.UrlDesc) && entity.UrlDesc.Length > 250)
			{
				entity.UrlDesc = entity.UrlDesc.Substring(0, 250);
			}

			await _unitOfWork.Repository<UrlCrawl>().AddAsync(entity);

			await _unitOfWork.Save(cancellationToken);

			return await Result<int>.SuccessAsync(entity.Id, "Thêm mới thành công.");
		}
	}
}
