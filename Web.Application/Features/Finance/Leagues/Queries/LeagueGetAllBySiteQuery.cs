using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Features.Finance.Leagues.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Leagues.Queries
{
    public class LeagueGetAllBySiteQuery : IRequest<List<LeagueGetAllBySiteDto>>
    {
        public short? SiteId { get; set; }
    }
    internal class LeagueGetAllBySiteQueryHandler : IRequestHandler<LeagueGetAllBySiteQuery, List<LeagueGetAllBySiteDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public LeagueGetAllBySiteQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<List<LeagueGetAllBySiteDto>> Handle(LeagueGetAllBySiteQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<League>().Entities.Where(x => x.SiteId == request.SiteId);
            var result = await query
                 .ProjectTo<LeagueGetAllBySiteDto>(_mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);
            return result;
        }
    }
}
