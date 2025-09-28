using AutoMapper;
using MediatR;
using Web.Application.Features.Finance.LeaveRequests.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.LeaveRequests.Queries
{
    public class LeaveRequestGetByIdQuery : IRequest<LeaveRequestGetByIdDto>
    {
        public int LeaveRequestId { get; set; }
    }
    internal class LeaveRequestGetByIdQueryHandler : IRequestHandler<LeaveRequestGetByIdQuery, LeaveRequestGetByIdDto>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public LeaveRequestGetByIdQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<LeaveRequestGetByIdDto> Handle(LeaveRequestGetByIdQuery queryInput, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<LeaveRequest>().Entities.FirstOrDefault(x => x.LeaveRequestId == queryInput.LeaveRequestId);
            if (entity == null)
            {
                return new LeaveRequestGetByIdDto();
            }
            var dataGetByIdDto = _mapper.Map<LeaveRequestGetByIdDto>(entity);
            return dataGetByIdDto;
        }
    }
}