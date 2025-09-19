using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Features.Finance.Matchs.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Matchs.Queries
{
    public class MatchGetScheduleQuery : IRequest<List<MatchGetAllDto>>
    {
        public short? LeagueId { get; set; }
        public DateTime? EstimateStartTime { get; set; }
    }

    internal class MatchGetScheduleQueryHandler : IRequestHandler<MatchGetScheduleQuery, List<MatchGetAllDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MatchGetScheduleQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<MatchGetAllDto>> Handle(MatchGetScheduleQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<Match>().Entities.AsNoTracking();
            if (request.LeagueId.HasValue)
                query = query.Where(m => m.LeagueId == request.LeagueId.Value);
            if (request.EstimateStartTime.HasValue)
            {
                var date = request.EstimateStartTime.Value.Date;
                query = query.Where(m => m.EstimateStartTime.HasValue
                                         && m.EstimateStartTime.Value.Date == date);
            }
            else
            {
                var now = DateTime.UtcNow;
                query = query.Where(m => m.EstimateStartTime >= now);
            }

            var result = await query
                 .ProjectTo<MatchGetAllDto>(_mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);

            return result ?? new List<MatchGetAllDto>();
        }
    }
}
