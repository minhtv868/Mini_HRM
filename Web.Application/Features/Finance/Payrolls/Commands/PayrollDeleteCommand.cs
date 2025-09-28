using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Payrolls.Commands
{
    public class PayrollDeleteCommand : IRequest<Result<int>>
    {
        public int PayrollId { get; set; }
    }
    internal class PayrollDeleteCommandHandler : IRequestHandler<PayrollDeleteCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        public PayrollDeleteCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ISender sender)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(PayrollDeleteCommand command, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<Payroll>().Entities.AsNoTracking().FirstOrDefault(x => x.PayrollId == command.PayrollId);
            if (entity == null)
            {
                return await Result<int>.FailureAsync("Payroll không tồn tại");
            }
            await _unitOfWork.Repository<Payroll>().DeleteAsync(entity);

            var deleteResult = await _unitOfWork.Save(cancellationToken);
            if (deleteResult > 0)
            {
                return await Result<int>.SuccessAsync($"Xóa dữ liệu thành công ");
            }

            return await Result<int>.FailureAsync($"Xóa dữ liệu không thành công ");
        }
    }
}