using AutoMapper;
using AutoMapper.QueryableExtensions;
using Web.Application.Features.BongDa24hCrawls.TemporaryDatas.DTOs;
using Web.Application.Interfaces.Repositories.BongDa24hCrawls;
using Web.Domain.Entities.Crawls;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Web.Application.Features.BongDa24hCrawls.TemporaryDatas.Queries
{
    public record TemporaryDataGetByTypeQuery : IRequest<List<TemporaryDataDto>>
    {
        public string DataSouceName { get; set; }
        public string DataAction { get; set; }
    }

    internal class TemporaryDataGetByTypeQueryHandler : IRequestHandler<TemporaryDataGetByTypeQuery, List<TemporaryDataDto>>
    {
        private readonly IBongDa24HCrawlUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TemporaryDataGetByTypeQueryHandler(IBongDa24HCrawlUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<TemporaryDataDto>> Handle(TemporaryDataGetByTypeQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<TemporaryData>().Entities.AsNoTracking().Where(x => x.DataSouceName == request.DataSouceName && x.DataAction == request.DataAction);

            var result = await query
                   .ProjectTo<TemporaryDataDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);

            return result;
        }
    }
}
