using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System.Linq.Dynamic.Core;
using Web.Application.DTOs.MediatR;
using Web.Application.Extensions;
using Web.Application.Features.Finance.Medias.DTOs;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Medias.Queries
{
    public record MediaGetPageQuery : BaseGetPageQuery, IRequest<PaginatedResult<MediaGetPageDto>>
    {
        public int? CategoryId { get; set; }
        public int? SiteId { get; set; }
        public byte? DataTypeId { get; set; }
        public byte? MediaTypeId { get; set; }
        public byte? Status { get; set; }
    }
    internal class MediaGetPageQueryHandler : IRequestHandler<MediaGetPageQuery, PaginatedResult<MediaGetPageDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        private readonly IAuditableService _auditableService;
        public MediaGetPageQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender, IAuditableService auditableService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
            _auditableService = auditableService;
        }

        public async Task<PaginatedResult<MediaGetPageDto>> Handle(MediaGetPageQuery queryInput, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<Media>().Entities;
            if (queryInput.SiteId > 0)
            {
                query = query.Where(x => x.SiteId == queryInput.SiteId);
            }
            //if (!string.IsNullOrWhiteSpace(queryInput.Keywords))
            //{
            //    query = query.Where(x => x.MessageName.Contains(queryInput.Keywords) || x.SendFrom.Contains(queryInput.Keywords) || x.Title.Contains(queryInput.Keywords));
            //}
            var result = await query.OrderBy(x => x.CrDateTime).ProjectTo<MediaGetPageDto>(_mapper.ConfigurationProvider).ToPaginatedListAsync(queryInput.Page, queryInput.PageSize, cancellationToken);
            await _auditableService.UpdateAuditableInfoAsync(result.Data);
            return result;
        }
    }
}
