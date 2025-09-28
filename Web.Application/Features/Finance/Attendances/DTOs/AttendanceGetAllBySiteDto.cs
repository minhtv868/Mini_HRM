using Web.Application.Common.Mappings;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Attendances.DTOs
{
    public class AttendanceGetAllBySiteDto : IMapFrom<Attendance>
    {
        public int AttendanceId { get; set; }
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

        // Ghi chú / trạng thái
        public string Notes { get; set; }
        public byte StatusId { get; set; }
        public int? SiteId { get; set; }
        public int? CrUserId { get; set; }
        public DateTime? CrDateTime { get; set; }
        public int? UpdUserId { get; set; }
        public DateTime? UpdDateTime { get; set; }
    }
}