using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Interfaces.Repositories.BongDa24hCrawls;
using Web.Domain.Entities.Crawls;
using Web.Domain.Enums.Bongda;

namespace Web.Application.Features.BongDa24hCrawls.GetCountData
{
    public class GetErrorFSPlayerCrawlQuery : IRequest<int>
    {
    }
    internal class GetErrorFSPlayerCrawlQueryHandler : IRequestHandler<GetErrorFSPlayerCrawlQuery, int>
    {
        private readonly IMapper _mapper;
        private readonly IBongDa24HCrawlUnitOfWork _unitOfWork;
        public GetErrorFSPlayerCrawlQueryHandler(IMapper mapper, IBongDa24HCrawlUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public Task<int> Handle(GetErrorFSPlayerCrawlQuery request, CancellationToken cancellationToken)
        {
            var result = _unitOfWork.Repository<FSPlayerCrawl>().Entities.AsNoTracking().Where(x => x.ProcessStatusId == 0 || x.ProcessStatusId == (byte)ProcessStatusEnum.CrawlError || x.ProcessStatusId == (byte)ProcessStatusEnum.ParseError).Count();
            return Task.FromResult(result);
        }
    }
}
