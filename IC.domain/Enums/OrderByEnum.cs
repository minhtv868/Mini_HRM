using System.ComponentModel.DataAnnotations;

namespace IC.Domain.Enums
{
    public enum OrderByEnum
    {
        [Display(Name = "Sắp xếp theo tên")]
        Name = 1,

        [Display(Name = "Sắp xếp theo ngày tạo")]
        CrDateTime = 2
    }
}
