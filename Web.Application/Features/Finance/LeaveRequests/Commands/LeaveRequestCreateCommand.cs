using AutoMapper;
using MediatR;
using System.ComponentModel;
using Web.Application.Common.Mappings;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.LeaveRequests.Commands
{
    public class LeaveRequestCreateCommand : IRequest<Result<int>>, IMapFrom<LeaveRequest>
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

        [DisplayName("Thêm tiếp dữ liệu khác")]
        public bool AddMoreData { get; set; }
    }
    internal class LeaveRequestCreateCommandHandler : IRequestHandler<LeaveRequestCreateCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ISender _sender;
        public LeaveRequestCreateCommandHandler(IFinanceUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService, ISender sender)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _sender = sender;
        }
        public async Task<Result<int>> Handle(LeaveRequestCreateCommand command, CancellationToken cancellationToken)
        {
            var entityAny = _unitOfWork.Repository<LeaveRequest>().Entities.FirstOrDefault(x => x.UserId == command.UserId &&
            x.StartDate == command.StartDate && x.EndDate == command.EndDate
            && x.SiteId == command.SiteId);
            if (entityAny != null)
            {
                return await Result<int>.FailureAsync($"LeaveRequest đã tồn tại");
            }
            var entity = _mapper.Map<LeaveRequest>(command);
            entity.CrUserId = _currentUserService.UserId;
            entity.CrDateTime = DateTime.Now;
            await _unitOfWork.Repository<LeaveRequest>().AddAsync(entity);
            var result = await _unitOfWork.Save(cancellationToken);
            if (result > 0)
            {
                return await Result<int>.SuccessAsync($"Thêm dữ liệu thành công");
            }
            return await Result<int>.FailureAsync($"Thêm dữ liệu không thành công");
        }
    }
}