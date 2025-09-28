using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Features.Finance.Sites.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Sites.Queries
{
    public class SiteGetAllQuery : IRequest<List<SiteGetAllDto>>
    {
    }
    internal class SiteGetAllQueryHandler : IRequestHandler<SiteGetAllQuery, List<SiteGetAllDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public SiteGetAllQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<List<SiteGetAllDto>> Handle(SiteGetAllQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<Site>().Entities.AsNoTracking();
            var result = await query
                 .ProjectTo<SiteGetAllDto>(_mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);
            return result;
        }
    }
}
