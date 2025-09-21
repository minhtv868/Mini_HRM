using AutoMapper;
using MediatR;
using Web.Application.Features.Finance.Leagues.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Leagues.Queries
{
    public class LeagueGetByUrlQuery : IRequest<Result<LeagueGetByUrlDto>>
    {
        public string LeagueUrl { get; set; }
    }
    internal class LeagueGetByUrlQueryHandler : IRequestHandler<LeagueGetByUrlQuery, Result<LeagueGetByUrlDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public LeagueGetByUrlQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<Result<LeagueGetByUrlDto>> Handle(LeagueGetByUrlQuery queryInput, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<League>().Entities.FirstOrDefault(x => x.LeagueUrl == queryInput.LeagueUrl);
            if (entity == null)
            {
                return await Result<LeagueGetByUrlDto>.FailureAsync("League không tồn tại");
            }
            var dataGetByIdDto = _mapper.Map<LeagueGetByUrlDto>(entity);
            return await Result<LeagueGetByUrlDto>.SuccessAsync(dataGetByIdDto);
        }
    }
}
