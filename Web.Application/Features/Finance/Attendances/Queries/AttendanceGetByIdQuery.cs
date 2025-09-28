using AutoMapper;
using MediatR;
using Web.Application.Features.Finance.Attendances.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Attendances.Queries
{
    public class AttendanceGetByIdQuery : IRequest<AttendanceGetByIdDto>
    {
        public int AttendanceId { get; set; }
    }
    internal class AttendanceGetByIdQueryHandler : IRequestHandler<AttendanceGetByIdQuery, AttendanceGetByIdDto>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public AttendanceGetByIdQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<AttendanceGetByIdDto> Handle(AttendanceGetByIdQuery queryInput, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<Attendance>().Entities.FirstOrDefault(x => x.AttendanceId == queryInput.AttendanceId);
            if (entity == null)
            {
                return new AttendanceGetByIdDto();
            }
            var dataGetByIdDto = _mapper.Map<AttendanceGetByIdDto>(entity);
            return dataGetByIdDto;
        }
    }
}