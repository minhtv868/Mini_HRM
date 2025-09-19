using AutoMapper;
using MediatR;
using Web.Application.Features.Finance.Sites.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Sites.Queries
{
    public class SiteGetByIdQuery : IRequest<Result<SiteGetByIdDto>>
    {
        public short SiteId { get; set; }
    }
    internal class SiteGetByIdQueryHandler : IRequestHandler<SiteGetByIdQuery, Result<SiteGetByIdDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public SiteGetByIdQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<Result<SiteGetByIdDto>> Handle(SiteGetByIdQuery queryInput, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<Site>().Entities.FirstOrDefault(x => x.SiteId == queryInput.SiteId);
            if (entity == null)
            {
                return await Result<SiteGetByIdDto>.FailureAsync("Site không tồn tại");
            }
            var dataGetByIdDto = _mapper.Map<SiteGetByIdDto>(entity);
            return await Result<SiteGetByIdDto>.SuccessAsync(dataGetByIdDto);
        }
    }
}
