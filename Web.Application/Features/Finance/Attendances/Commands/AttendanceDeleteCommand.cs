using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Attendances.Commands
{
    public class AttendanceDeleteCommand : IRequest<Result<int>>
    {
        public int AttendanceId { get; set; }
    }
    internal class AttendanceDeleteCommandHandler : IRequestHandler<AttendanceDeleteCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        public AttendanceDeleteCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ISender sender)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(AttendanceDeleteCommand command, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<Attendance>().Entities.AsNoTracking().FirstOrDefault(x => x.AttendanceId == command.AttendanceId);
            if (entity == null)
            {
                return await Result<int>.FailureAsync("Attendance không tồn tại");
            }
            await _unitOfWork.Repository<Attendance>().DeleteAsync(entity);

            var deleteResult = await _unitOfWork.Save(cancellationToken);
            if (deleteResult > 0)
            {
                return await Result<int>.SuccessAsync($"Xóa dữ liệu thành công ");
            }

            return await Result<int>.FailureAsync($"Xóa dữ liệu không thành công ");
        }
    }
}