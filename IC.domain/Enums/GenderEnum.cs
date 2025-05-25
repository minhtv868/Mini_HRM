using System.ComponentModel.DataAnnotations;

namespace IC.Domain.Enums
{
	public enum GenderEnum
	{
		[Display(Name = "Nam")]
		Male,

		[Display(Name = "Nữ")]
		Female,

		[Display(Name = "Khác")]
		Others
	}
}
