using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Web.Domain.Enums
{
	public enum SearchByDateEnum
	{
		[Display(Name = "Ngày tạo")]
		CreatedDate = 1,

		[Display(Name = "Ngày cập nhật")]
		UpdatedDate = 2
	}
}
