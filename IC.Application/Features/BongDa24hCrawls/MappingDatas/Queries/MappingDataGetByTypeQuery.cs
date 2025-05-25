using AutoMapper;
using AutoMapper.QueryableExtensions;
using IC.Application.Features.BongDa24hCrawls.MappingDatas.DTOs;
using IC.Application.Interfaces.Repositories.BongDa24hCrawls;
using IC.Domain.Entities.BongDa24hCrawls;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace IC.Application.Features.BongDa24hCrawls.MappingDatas.Queries
{
    public record MappingDataGetByTypeQuery : IRequest<List<MappingDataDto>>
    {
        public string DataSouceName { get; set; }
        public string DataAction { get; set; }
    }

    internal class MappingDataGetByTypeQueryHandler : IRequestHandler<MappingDataGetByTypeQuery, List<MappingDataDto>>
    {
        private readonly IBongDa24HCrawlUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MappingDataGetByTypeQueryHandler(IBongDa24HCrawlUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<MappingDataDto>> Handle(MappingDataGetByTypeQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<MappingData>().Entities.AsNoTracking();

            var result = await query
                   .ProjectTo<MappingDataDto>(_mapper.ConfigurationProvider)
                   .ToListAsync(cancellationToken);

            return result;
        }
    }
}
