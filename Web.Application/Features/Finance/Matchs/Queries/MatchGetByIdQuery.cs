using AutoMapper;
using MediatR;
using Web.Application.Features.Finance.Matchs.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Matchs.Queries
{
    public class MatchGetByIdQuery : IRequest<Result<MatchGetByIdDto>>
    {
        public short MatchId { get; set; }
    }
    internal class MatchGetByIdQueryHandler : IRequestHandler<MatchGetByIdQuery, Result<MatchGetByIdDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public MatchGetByIdQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<Result<MatchGetByIdDto>> Handle(MatchGetByIdQuery queryInput, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<Match>().Entities.FirstOrDefault(x => x.MatchId == queryInput.MatchId);
            if (entity == null)
            {
                return await Result<MatchGetByIdDto>.FailureAsync("Match không tồn tại");
            }
            var dataGetByIdDto = _mapper.Map<MatchGetByIdDto>(entity);
            return await Result<MatchGetByIdDto>.SuccessAsync(dataGetByIdDto);
        }
    }
}
