using AutoMapper;
using MediatR;
using Web.Application.Features.Finance.Departments.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Departments.Queries
{
    public class DepartmentGetByIdQuery : IRequest<DepartmentGetByIdDto>
    {
        public int DepartmentId { get; set; }
    }
    internal class DepartmentGetByIdQueryHandler : IRequestHandler<DepartmentGetByIdQuery, DepartmentGetByIdDto>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public DepartmentGetByIdQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<DepartmentGetByIdDto> Handle(DepartmentGetByIdQuery queryInput, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<Department>().Entities.FirstOrDefault(x => x.DepartmentId == queryInput.DepartmentId);
            if (entity == null)
            {
                return new DepartmentGetByIdDto();
            }
            var dataGetByIdDto = _mapper.Map<DepartmentGetByIdDto>(entity);
            return dataGetByIdDto;
        }
    }
}
