using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Features.Finance.Payrolls.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Payrolls.Queries
{
    public class PayrollGetAllBySiteQuery : IRequest<List<PayrollGetAllBySiteDto>>
    {
        public int? SiteId { get; set; }
    }
    internal class PayrollGetAllBySiteQueryHandler : IRequestHandler<PayrollGetAllBySiteQuery, List<PayrollGetAllBySiteDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public PayrollGetAllBySiteQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<List<PayrollGetAllBySiteDto>> Handle(PayrollGetAllBySiteQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<Payroll>().Entities.Where(x => x.SiteId == request.SiteId);
            var result = await query
                 .ProjectTo<PayrollGetAllBySiteDto>(_mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);
            return result;
        }
    }
}