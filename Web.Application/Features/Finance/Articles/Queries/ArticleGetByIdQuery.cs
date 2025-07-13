using AutoMapper;
using MediatR;
using Web.Application.Features.Finance.Articles.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Articles.Queries
{
    public class ArticleGetByIdQuery : IRequest<ArticleGetByIdDto>
    {
        public int ArticleId { get; set; }
    }
    internal class ArticleGetByIdQueryHandler : IRequestHandler<ArticleGetByIdQuery, ArticleGetByIdDto>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public ArticleGetByIdQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<ArticleGetByIdDto> Handle(ArticleGetByIdQuery queryInput, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<Article>().Entities.FirstOrDefault(x => x.Id == queryInput.ArticleId);
            if (entity == null)
            {
                return new ArticleGetByIdDto();
            }
            var dataGetByIdDto = _mapper.Map<ArticleGetByIdDto>(entity);
            return dataGetByIdDto;
        }
    }
}
