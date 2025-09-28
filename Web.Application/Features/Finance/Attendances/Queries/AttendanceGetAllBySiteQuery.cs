using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Features.Finance.Attendances.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Attendances.Queries
{
    public class AttendanceGetAllBySiteQuery : IRequest<List<AttendanceGetAllBySiteDto>>
    {
        public int? SiteId { get; set; }
    }
    internal class AttendanceGetAllBySiteQueryHandler : IRequestHandler<AttendanceGetAllBySiteQuery, List<AttendanceGetAllBySiteDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public AttendanceGetAllBySiteQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<List<AttendanceGetAllBySiteDto>> Handle(AttendanceGetAllBySiteQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<Attendance>().Entities.Where(x => x.SiteId == request.SiteId);
            var result = await query
                 .ProjectTo<AttendanceGetAllBySiteDto>(_mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);
            return result;
        }
    }
}