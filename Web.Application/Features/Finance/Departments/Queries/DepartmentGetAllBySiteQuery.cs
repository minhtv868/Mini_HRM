using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Features.Finance.Departments.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Departments.Queries
{
    public class DepartmentGetAllBySiteQuery : IRequest<List<DepartmentGetAllBySiteDto>>
    {
        public int? SiteId { get; set; }
    }
    internal class DepartmentGetAllBySiteQueryHandler : IRequestHandler<DepartmentGetAllBySiteQuery, List<DepartmentGetAllBySiteDto>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public DepartmentGetAllBySiteQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<List<DepartmentGetAllBySiteDto>> Handle(DepartmentGetAllBySiteQuery request, CancellationToken cancellationToken)
        {
            var query = _unitOfWork.Repository<Department>().Entities.Where(x => x.SiteId == request.SiteId);
            var result = await query
                 .ProjectTo<DepartmentGetAllBySiteDto>(_mapper.ConfigurationProvider)
                 .ToListAsync(cancellationToken);
            return result;
        }
    }
}
