using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using System.ComponentModel;
using System.Linq.Dynamic.Core;
using Web.Application.DTOs.MediatR;
using Web.Application.Extensions;
using Web.Application.Features.Finance.Payrolls.DTOs;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Payrolls.Queries
{
    public record PayrollGetPageQuery : BaseGetPageQuery, IRequest<PaginatedResult<PayrollGetPageDto>>
    {
        [DisplayName("Site")]
        public int? SiteId { get; set; }
    }
    internal class PayrollGetPageQueryHandler : IRequestHandler<PayrollGetPageQuery, PaginatedResult<PayrollGetPageDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        private readonly IAuditableService _auditableService;
        public PayrollGetPageQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender, IAuditableService auditableService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
            _auditableService = auditableService;
        }

        public async Task<PaginatedResult<PayrollGetPageDto>> Handle(PayrollGetPageQuery queryInput, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<Payroll>().Entities;
            if (queryInput.SiteId > 0)
            {
                query = query.Where(x => x.SiteId == queryInput.SiteId);
            }
            if (!string.IsNullOrWhiteSpace(queryInput.Keywords))
            {
                //   query = query.Where(x => x.PayrollName.Contains(queryInput.Keywords) || x.PayrollDesc.Contains(queryInput.Keywords));
            }
            var result = await query.OrderBy(x => x.CrDateTime).ProjectTo<PayrollGetPageDto>(_mapper.ConfigurationProvider).ToPaginatedListAsync(queryInput.Page, queryInput.PageSize, cancellationToken);
            await _auditableService.UpdateAuditableInfoAsync(result.Data);
            return result;
        }
    }
}