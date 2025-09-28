using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using Web.Application.Common.Mappings;
using Web.Application.Features.Finance.LeaveRequests.DTOs;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.LeaveRequests.Commands
{
    public class LeaveRequestEditCommand : IRequest<Result<int>>, IMapFrom<LeaveRequest>, IMapFrom<LeaveRequestGetByIdDto>
    {
        public int LeaveRequestId { get; set; }

        [DisplayName("Nhân viên")]
        public int UserId { get; set; }

        [DisplayName("Ngày bắt đầu")]
        public DateTime StartDate { get; set; }

        [DisplayName("Ngày kết thúc")]
        public DateTime EndDate { get; set; }

        [DisplayName("Lý do")]
        public string Reason { get; set; }

        [DisplayName("Trạng thái")]
        public byte? Status { get; set; }

        [DisplayName("Site")]
        public int? SiteId { get; set; }
    }
    internal class LeaveRequestEditCommandHandler : IRequestHandler<LeaveRequestEditCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISender _sender;
        public LeaveRequestEditCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService,
            ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _sender = sender;
        }
        public async Task<Result<int>> Handle(LeaveRequestEditCommand command, CancellationToken cancellationToken)
        {
            var entity = _unitOfWork.Repository<LeaveRequest>().Entities.AsNoTracking().FirstOrDefault(x => x.LeaveRequestId == command.LeaveRequestId);
            if (entity == null)
            {
                return await Result<int>.FailureAsync("LeaveRequest không tồn tại");
            }
            //if (command.LeaveRequestName != entity.LeaveRequestName)
            //{
            //    var existing = await _unitOfWork.Repository<LeaveRequest>().Entities
            // .Where(x => x.SiteId == command.SiteId)
            // .AsNoTracking()
            // .ToListAsync();
            //    var existing2 = existing.FirstOrDefault(x => string.Equals(x.LeaveRequestName, command.LeaveRequestName, StringComparison.Ordinal));
            //    if (existing2 != null)
            //    {
            //        return await Result<int>.FailureAsync("LeaveRequest này đã tồn tại. Vui lòng chọn tên khác.");
            //    }
            //}
            entity = _mapper.Map<LeaveRequest>(command);
            entity.UpdDateTime = DateTime.Now;
            entity.UpdUserId = _currentUserService.UserId;
            await _unitOfWork.Repository<LeaveRequest>().UpdateFieldsAsync(entity,
                x => x.UserId,
                x => x.StartDate,
                x => x.EndDate,
                x => x.Reason,
                x => x.UpdUserId,
                x => x.UpdDateTime
              );
            var result = await _unitOfWork.Save(cancellationToken);
            if (result > 0)
            {
                return await Result<int>.SuccessAsync("Cập nhật dữ liệu thành công.");
            }
            return await Result<int>.FailureAsync("Cập nhật dữ liệu không thành công.");
        }
    }
}