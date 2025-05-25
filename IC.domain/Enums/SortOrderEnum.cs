using System.ComponentModel.DataAnnotations;

namespace IC.Domain.Enums
{
	public enum SortOrderEnum
	{
		[Display(Name = "Sắp xếp giảm dần")]
		Descending = 1,

		[Display(Name = "Sắp xếp tăng dần")]
		Ascending = 2
	}
}
