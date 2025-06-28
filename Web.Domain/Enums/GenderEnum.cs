using System.ComponentModel.DataAnnotations;

namespace Web.Domain.Enums
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
