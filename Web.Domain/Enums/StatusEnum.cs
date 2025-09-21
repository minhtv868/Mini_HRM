using System.ComponentModel.DataAnnotations;

namespace Web.Domain.Enums
{
    public enum StatusEnum
    {

        [Display(Name = "Hoạt động")]
        Active = 1,
        [Display(Name = "Dừng hoạt động")]
        InActive = 2,

        [Display(Name = "Chờ kích hoạt")]
        Pending = 3,

        [Display(Name = "Đã xóa")]
        Deleted = 4
    }

}
