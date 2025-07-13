using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Features.Finance.Articles.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Articles.Queries
{
    public class ArticleGetAllBySiteQuery : IRequest<List<ArticleGetAllBySiteDto>>
    {
        public short? SiteId { get; set; }
    }
    internal class ArticleGetAllBySiteQueryHandler : IRequestHandler<ArticleGetAllBySiteQuery, List<ArticleGetAllBySiteDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public ArticleGetAllBySiteQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<List<ArticleGetAllBySiteDto>> Handle(ArticleGetAllBySiteQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<Article>().Entities.Where(x => x.SiteId == request.SiteId);
            var result = await query
                 .ProjectTo<ArticleGetAllBySiteDto>(_mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);
            return result;
        }
    }
}
