using System.ComponentModel.DataAnnotations;

namespace Web.Domain.Enums
{
	public enum StatusEnum
	{
		[Display(Name = "Dừng Hoạt động")]
		InActive = 1,

		[Display(Name = "Hoạt động")]
		Active = 2,

        [Display(Name = "Chờ kích hoạt")]
        WaitActive = 5
    }
}
