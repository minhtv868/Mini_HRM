using AutoMapper;
using MediatR;
using Web.Application.Features.Finance.Leagues.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Leagues.Queries
{
    public class LeagueGetByIdQuery : IRequest<Result<LeagueGetByIdDto>>
    {
        public short LeagueId { get; set; }
    }
    internal class LeagueGetByIdQueryHandler : IRequestHandler<LeagueGetByIdQuery, Result<LeagueGetByIdDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public LeagueGetByIdQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<Result<LeagueGetByIdDto>> Handle(LeagueGetByIdQuery queryInput, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<League>().Entities.FirstOrDefault(x => x.LeagueId == queryInput.LeagueId);
            if (entity == null)
            {
                return await Result<LeagueGetByIdDto>.FailureAsync("League không tồn tại");
            }
            var dataGetByIdDto = _mapper.Map<LeagueGetByIdDto>(entity);
            return await Result<LeagueGetByIdDto>.SuccessAsync(dataGetByIdDto);
        }
    }
}
