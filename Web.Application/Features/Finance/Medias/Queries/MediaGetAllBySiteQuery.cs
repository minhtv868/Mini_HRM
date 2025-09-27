using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Features.Finance.Medias.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Medias.Queries
{
    public class MediaGetAllBySiteQuery : IRequest<List<MediaGetAllBySiteDto>>
    {
        public short? SiteId { get; set; }
    }
    internal class MediaGetAllBySiteQueryHandler : IRequestHandler<MediaGetAllBySiteQuery, List<MediaGetAllBySiteDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public MediaGetAllBySiteQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<List<MediaGetAllBySiteDto>> Handle(MediaGetAllBySiteQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<Media>().Entities.Where(x => x.SiteId == request.SiteId);
            var result = await query
                 .ProjectTo<MediaGetAllBySiteDto>(_mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);
            return result;
        }
    }
}
