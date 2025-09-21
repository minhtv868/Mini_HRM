using AutoMapper;
using MediatR;
using Web.Application.Features.Finance.Seos.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Seos.Queries
{
    public class SeoGetByIdQuery : IRequest<Result<SeoGetByIdDto>>
    {
        public short SeoId { get; set; }
    }
    internal class SeoGetByIdQueryHandler : IRequestHandler<SeoGetByIdQuery, Result<SeoGetByIdDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public SeoGetByIdQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<Result<SeoGetByIdDto>> Handle(SeoGetByIdQuery queryInput, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<Seo>().Entities.FirstOrDefault(x => x.SeoId == queryInput.SeoId);
            if (entity == null)
            {
                return await Result<SeoGetByIdDto>.FailureAsync("Seo không tồn tại");
            }
            var dataGetByIdDto = _mapper.Map<SeoGetByIdDto>(entity);
            return await Result<SeoGetByIdDto>.SuccessAsync(dataGetByIdDto);
        }
    }
}
