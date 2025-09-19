using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Features.Finance.Teams.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Teams.Queries
{
    public class TeamGetAllQuery : IRequest<List<TeamGetAllDto>>
    {

    }
    internal class TeamGetAllQueryHandler : IRequestHandler<TeamGetAllQuery, List<TeamGetAllDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public TeamGetAllQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<List<TeamGetAllDto>> Handle(TeamGetAllQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<Team>().Entities.AsNoTracking();
            var result = await query
                 .ProjectTo<TeamGetAllDto>(_mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);
            return result;
        }
    }
}
