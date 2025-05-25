using System.ComponentModel.DataAnnotations;

namespace IC.Domain.Enums
{
	public enum ActiveEnum
	{
		[Display(Name = "Hủy kích hoạt")]
		Deactivate = 0,

		[Display(Name = "Kích hoạt")]
		Activated = 1
	}
}
