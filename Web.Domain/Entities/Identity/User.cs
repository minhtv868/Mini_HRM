using Microsoft.AspNetCore.Identity;

namespace Web.Domain.Entities.Identity
{
    public class User : IdentityUser<int>
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public DateTime? BirthDay { get; set; }
        public int? DefaultActionId { get; set; }
        public string Notes { get; set; }
        public string AvatarPath { get; set; }
        public bool? IsEnabled { get; set; }
        //    public int? SiteId { get; set; }
        public DateTime? LastTimeChangePass { get; set; }
        public DateTime? LastTimeLogin { get; set; }
        public DateTime? CrDateTime { get; set; }

        // Thông tin nhân viên
        //public string EmployeeCode { get; set; }
        //public int? DepartmentId { get; set; }
        //public int? PositionId { get; set; }
        //public string PhoneNumberStr { get; set; }
        //public string Gender { get; set; }
        //public DateTime? JoinDate { get; set; }
        //public DateTime? EndDate { get; set; }
        //public string IdentityNumber { get; set; }
        //public DateTime? IdentityDate { get; set; }
        //public string IdentityPlace { get; set; }

        //// Thông tin ngân hàng (nếu cần trả lương tự động)
        //public string BankAccountNumber { get; set; }
        //public string BankName { get; set; }
    }
}
