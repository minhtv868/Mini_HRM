using System.ComponentModel.DataAnnotations;

namespace IC.Domain.Enums
{
    public enum ItemPositionEnum
	{
        [Display(Name = "Cuối trong danh sách")]
        First = 1,

        [Display(Name = "Đầu danh sách")]
        Last = 2,

        [Display(Name = "Sau mục")]
		After = 3,

        [Display(Name = "Theo số TT")]
		PositionNumber = 4,

    }
}
