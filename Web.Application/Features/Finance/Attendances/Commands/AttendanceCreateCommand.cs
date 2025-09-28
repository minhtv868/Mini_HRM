using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Security;
using Web.Application.Common.Mappings;
using Web.Application.Interfaces;
using Web.Application.Interfaces.Repositories.Finances;
using Web.Domain.Entities.Finance;
using Web.Shared;

namespace Web.Application.Features.Finance.Attendances.Commands
{
    public class AttendanceCreateCommand : IRequest<Result<int>>, IMapFrom<Attendance>
    {
        public int UserId { get; set; }
        public DateTime WorkDate { get; set; }
        public TimeSpan? CheckIn { get; set; }
        public TimeSpan? CheckOut { get; set; }
        public double WorkHours { get; set; }
        public string CheckInIp { get; set; }
        public string CheckOutIp { get; set; }
        public string CheckInDevice { get; set; }   // mobile/web/desktop
        public string CheckOutDevice { get; set; }
        public string CheckInLocation { get; set; } // GPS hoặc địa chỉ
        public string CheckOutLocation { get; set; }

        public string Notes { get; set; }
        public byte StatusId { get; set; }

        [DisplayName("Site")]
        public int? SiteId { get; set; }

        [DisplayName("Thêm tiếp dữ liệu khác")]
        public bool AddMoreData { get; set; }
    }

    internal class AttendanceCreateCommandHandler : IRequestHandler<AttendanceCreateCommand, Result<int>>
    {
        private readonly IFinanceUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public AttendanceCreateCommandHandler(
            IFinanceUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<Result<int>> Handle(AttendanceCreateCommand command, CancellationToken cancellationToken)
        {
            var repo = _unitOfWork.Repository<Attendance>();
            var existing = await repo.Entities
                .FirstOrDefaultAsync(x => x.UserId == command.UserId && x.WorkDate == command.WorkDate, cancellationToken);

            // CheckIn
            if (command.CheckIn.HasValue)
            {
                ValidateCheckIn(existing, command.WorkDate, command.CheckInIp);

                if (existing == null)
                {
                    var entity = _mapper.Map<Attendance>(command);
                    entity.CrUserId = _currentUserService.UserId;
                    entity.CrDateTime = DateTime.Now;
                    await repo.AddAsync(entity);
                }
                else
                {
                    existing.CheckIn = command.CheckIn;
                    existing.CheckInIp = command.CheckInIp;
                    existing.CheckInDevice = command.CheckInDevice;
                    existing.UpdUserId = _currentUserService.UserId;
                    existing.UpdDateTime = DateTime.Now;
                    await repo.UpdateFieldsAsync(existing, x => x.CheckIn, x => x.CheckInIp, x => x.CheckInDevice, x => x.UpdUserId, x => x.UpdDateTime);
                }
            }

            // CheckOut
            if (command.CheckOut.HasValue)
            {
                ValidateCheckOut(existing, command.CheckOut.Value, command.CheckOutIp);

                if (existing != null)
                {
                    existing.CheckOut = command.CheckOut;
                    existing.CheckOutIp = command.CheckOutIp;
                    existing.CheckOutDevice = command.CheckOutDevice;
                    existing.WorkHours = (command.CheckOut.Value - existing.CheckIn.Value).TotalHours;
                    existing.UpdUserId = _currentUserService.UserId;
                    existing.UpdDateTime = DateTime.Now;
                    await repo.UpdateFieldsAsync(existing, x => x.CheckOut, x => x.CheckOutIp, x => x.CheckOutDevice, x => x.WorkHours,
                    x => x.UpdUserId, x => x.UpdDateTime);
                }
            }

            var result = await _unitOfWork.Save(cancellationToken);
            if (result > 0)
                return await Result<int>.SuccessAsync("Thêm dữ liệu thành công");

            return await Result<int>.FailureAsync("Thêm dữ liệu không thành công");
        }

        // ======= Validation chống gian lận =======
        private void ValidateCheckIn(Attendance existing, DateTime workDate, string ip)
        {
            if (existing != null && existing.CheckIn != null)
                throw new InvalidOperationException("Đã check-in hôm nay rồi!");

            if (workDate.Date != DateTime.Today)
                throw new InvalidOperationException("Chỉ được check-in cho ngày hôm nay!");

            var now = DateTime.Now.TimeOfDay;
            if (now < new TimeSpan(7, 0, 0) || now > new TimeSpan(10, 0, 0))
                throw new InvalidOperationException("Chỉ được check-in từ 7:00 - 10:00!");

            if (string.IsNullOrEmpty(ip))
                throw new SecurityException("Không xác định được IP!");
        }

        private void ValidateCheckOut(Attendance existing, TimeSpan checkOut, string ip)
        {
            if (existing == null || existing.CheckIn == null)
                throw new InvalidOperationException("Chưa check-in, không thể check-out!");

            var workDuration = checkOut - existing.CheckIn.Value;
            if (workDuration.TotalHours < 8)
                throw new InvalidOperationException("Chưa đủ 8 tiếng làm việc!");

            if (checkOut > new TimeSpan(22, 0, 0))
                throw new InvalidOperationException("Không thể check-out sau 22:00!");

            if (string.IsNullOrEmpty(ip))
                throw new SecurityException("Không xác định được IP!");
        }
    }
}
