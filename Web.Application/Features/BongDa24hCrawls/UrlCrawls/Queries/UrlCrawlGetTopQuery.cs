using AutoMapper;
using AutoMapper.QueryableExtensions;
using Web.Application.Features.BongDa24hCrawls.UrlCrawls.DTOs;
using Web.Application.Interfaces.Repositories.BongDa24hCrawls;
using Web.Domain.Entities.Crawls;
using Web.Shared.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Web.Application.Features.BongDa24hCrawls.UrlCrawls.Queries
{
	public class UrlCrawlGetTopQuery : IRequest<List<UrlCrawlDto>>
	{
		public string UrlGroup { get; set; }
		public int PageSize { get; set; } = 10;
		public UrlCrawlGetTopQuery(string urlGroup, int pageSize)
		{
			UrlGroup = urlGroup;
			PageSize = pageSize;
		}
	}
	internal class UrlCrawlGetTopQueryHandler : IRequestHandler<UrlCrawlGetTopQuery, List<UrlCrawlDto>>
	{
		private readonly IMapper _mapper;
		private readonly IBongDa24HCrawlUnitOfWork _unitOfWork;
		public UrlCrawlGetTopQueryHandler(IMapper mapper, IBongDa24HCrawlUnitOfWork unitOfWork)
		{
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}
		public async Task<List<UrlCrawlDto>> Handle(UrlCrawlGetTopQuery request, CancellationToken cancellationToken)
		{
			//Lấy theo lô để xử lý
			//Tạo mã theo lô BatchCode
			var batchCode = StringHelper.GenerateUniqId();
			
			var sql = $"UPDATE UrlCrawls SET BatchCode=N'{batchCode}' WHERE Id IN (SELECT TOP({request.PageSize}) Id FROM UrlCrawls WHERE UrlGroup=N'{request.UrlGroup}' AND UrlType='ListPageRefresh' UNION SELECT TOP(2) Id FROM UrlCrawls WHERE UrlGroup=N'{request.UrlGroup}' AND UrlType='ListPage' AND BatchCode IS NULL UNION SELECT TOP({request.PageSize}) Id FROM UrlCrawls WHERE UrlGroup=N'{request.UrlGroup}' AND UrlType='DetailPage' AND BatchCode IS NULL AND IsCrawled = 0)";
			
			var rowCount = await _unitOfWork.Repository<UrlCrawl>().ExecNoneQuerySql(sql);

			if (rowCount > 0)
			{
				//await _unitOfWork.Repository<UrlCrawl>().ExecNoneQuerySql("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");

				var result = await _unitOfWork.Repository<UrlCrawl>().Entities.AsNoTracking()
				   .Where(x => x.BatchCode == batchCode)
				   .ProjectTo<UrlCrawlDto>(_mapper.ConfigurationProvider)
				   .ToListAsync(cancellationToken);

				return result;
			}

			return new List<UrlCrawlDto>();
		}
	}
}
