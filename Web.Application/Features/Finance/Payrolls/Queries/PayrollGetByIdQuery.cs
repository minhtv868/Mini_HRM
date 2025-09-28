using AutoMapper;
using MediatR;
using Web.Application.Features.Finance.Payrolls.DTOs;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Payrolls.Queries
{
    public class PayrollGetByIdQuery : IRequest<PayrollGetByIdDto>
    {
        public int PayrollId { get; set; }
    }
    internal class PayrollGetByIdQueryHandler : IRequestHandler<PayrollGetByIdQuery, PayrollGetByIdDto>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISender _sender;
        public PayrollGetByIdQueryHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _sender = sender;
        }
        public async Task<PayrollGetByIdDto> Handle(PayrollGetByIdQuery queryInput, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<Payroll>().Entities.FirstOrDefault(x => x.PayrollId == queryInput.PayrollId);
            if (entity == null)
            {
                return new PayrollGetByIdDto();
            }
            var dataGetByIdDto = _mapper.Map<PayrollGetByIdDto>(entity);
            return dataGetByIdDto;
        }
    }
}