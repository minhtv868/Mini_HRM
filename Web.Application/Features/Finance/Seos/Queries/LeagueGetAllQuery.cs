//using AutoMapper;
//using AutoMapper.QueryableExtensions;
//using MediatR;
//using Microsoft.EntityFrameworkCore;
//using Web.Application.Features.Finance.Leagues.DTOs;
//using Web.Application.Interfaces.Repositories.Finances;
//using Web.Domain.Entities.Finance;

//namespace Web.Application.Features.Finance.Leagues.Queries
//{
//    public class LeagueGetAllQuery : IRequest<List<LeagueGetAllDto>>
//    {

//    }
//    internal class LeagueGetAllQueryHandler : IRequestHandler<LeagueGetAllQuery, List<LeagueGetAllDto>>
//    {
//        private readonly IFinanceUnitOfWork _unitOfWork;
//        private readonly IMapper _mapper;
//        private readonly ISender _sender;
//        public LeagueGetAllQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
//        {
//            _unitOfWork = unitOfWork;
//            _mapper = mapper;
//            _sender = sender;
//        }
//        public async Task<List<LeagueGetAllDto>> Handle(LeagueGetAllQuery request, CancellationToken cancellationToken)
//        {
//            var query = _unitOfWork.Repository<League>().Entities.AsNoTracking();
//            var result = await query
//                 .ProjectTo<LeagueGetAllDto>(_mapper.ConfigurationProvider)
//                 .ToListAsync(cancellationToken);
//            return result;
//        }
//    }
//}
