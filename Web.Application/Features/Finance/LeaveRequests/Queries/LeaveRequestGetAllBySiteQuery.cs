using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Features.Finance.LeaveRequests.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.LeaveRequests.Queries
{
    public class LeaveRequestGetAllBySiteQuery : IRequest<List<LeaveRequestGetAllBySiteDto>>
    {
        public int? SiteId { get; set; }
    }
    internal class LeaveRequestGetAllBySiteQueryHandler : IRequestHandler<LeaveRequestGetAllBySiteQuery, List<LeaveRequestGetAllBySiteDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public LeaveRequestGetAllBySiteQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<List<LeaveRequestGetAllBySiteDto>> Handle(LeaveRequestGetAllBySiteQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<LeaveRequest>().Entities.Where(x => x.SiteId == request.SiteId);
            var result = await query
                 .ProjectTo<LeaveRequestGetAllBySiteDto>(_mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);
            return result;
        }
    }
}