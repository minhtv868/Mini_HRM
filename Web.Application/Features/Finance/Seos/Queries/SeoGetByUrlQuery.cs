using AutoMapper;
using MediatR;
using Web.Application.Features.Finance.Seos.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Seos.Queries
{
    public class SeoGetByUrlQuery : IRequest<Result<SeoGetByUrlDto>>
    {
        public string Url { get; set; }
    }
    internal class SeoGetByUrlQueryHandler : IRequestHandler<SeoGetByUrlQuery, Result<SeoGetByUrlDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public SeoGetByUrlQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<Result<SeoGetByUrlDto>> Handle(SeoGetByUrlQuery queryInput, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<Seo>().Entities.FirstOrDefault(x => x.Url.Contains(queryInput.Url));
            if (entity == null)
            {
                return await Result<SeoGetByUrlDto>.FailureAsync("Seo không tồn tại");
            }
            var dataGetByUrlDto = _mapper.Map<SeoGetByUrlDto>(entity);
            return await Result<SeoGetByUrlDto>.SuccessAsync(dataGetByUrlDto);
        }
    }
}
