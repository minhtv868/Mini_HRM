using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Features.Finance.Matchs.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Matchs.Queries
{
    public class MatchGetAllQuery : IRequest<List<MatchGetAllDto>>
    {

    }
    internal class MatchGetAllQueryHandler : IRequestHandler<MatchGetAllQuery, List<MatchGetAllDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public MatchGetAllQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<List<MatchGetAllDto>> Handle(MatchGetAllQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<Match>().Entities.AsNoTracking();
            var result = await query
                 .ProjectTo<MatchGetAllDto>(_mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);
            return result;
        }
    }
}
