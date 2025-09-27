using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System.ComponentModel;
using System.Linq.Dynamic.Core;
using Web.Application.DTOs.MediatR;
using Web.Application.Extensions;
using Web.Application.Features.Finance.Departments.DTOs;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Departments.Queries
{
    public record DepartmentGetPageQuery : BaseGetPageQuery, IRequest<PaginatedResult<DepartmentGetPageDto>>
    {
        [DisplayName("Site")]
        public int? SiteId { get; set; }
    }
    internal class DepartmentGetPageQueryHandler : IRequestHandler<DepartmentGetPageQuery, PaginatedResult<DepartmentGetPageDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        private readonly IAuditableService _auditableService;
        public DepartmentGetPageQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender, IAuditableService auditableService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
            _auditableService = auditableService;
        }

        public async Task<PaginatedResult<DepartmentGetPageDto>> Handle(DepartmentGetPageQuery queryInput, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<Department>().Entities;
            if (queryInput.SiteId > 0)
            {
                query = query.Where(x => x.SiteId == queryInput.SiteId);
            }
            if (!string.IsNullOrWhiteSpace(queryInput.Keywords))
            {
                query = query.Where(x => x.DepartmentName.Contains(queryInput.Keywords) || x.DepartmentDesc.Contains(queryInput.Keywords));
            }
            var result = await query.OrderBy(x => x.CrDateTime).ProjectTo<DepartmentGetPageDto>(_mapper.ConfigurationProvider).ToPaginatedListAsync(queryInput.Page, queryInput.PageSize, cancellationToken);
            await _auditableService.UpdateAuditableInfoAsync(result.Data);
            return result;
        }
    }
}
