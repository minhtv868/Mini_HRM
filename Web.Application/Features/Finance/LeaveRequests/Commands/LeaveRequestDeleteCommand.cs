using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.LeaveRequests.Commands
{
    public class LeaveRequestDeleteCommand : IRequest<Result<int>>
    {
        public int LeaveRequestId { get; set; }
    }
    internal class LeaveRequestDeleteCommandHandler : IRequestHandler<LeaveRequestDeleteCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        public LeaveRequestDeleteCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ISender sender)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(LeaveRequestDeleteCommand command, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<LeaveRequest>().Entities.AsNoTracking().FirstOrDefault(x => x.LeaveRequestId == command.LeaveRequestId);
            if (entity == null)
            {
                return await Result<int>.FailureAsync("LeaveRequest không tồn tại");
            }
            await _unitOfWork.Repository<LeaveRequest>().DeleteAsync(entity);

            var deleteResult = await _unitOfWork.Save(cancellationToken);
            if (deleteResult > 0)
            {
                return await Result<int>.SuccessAsync($"Xóa dữ liệu thành công ");
            }

            return await Result<int>.FailureAsync($"Xóa dữ liệu không thành công ");
        }
    }
}