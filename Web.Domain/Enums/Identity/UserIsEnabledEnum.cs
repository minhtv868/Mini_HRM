using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Web.Domain.Enums.Identity
{
	public enum UserIsEnabledEnum
	{
        [Display(Name = "Kích hoạt")]
        Active = 1,

        [Display(Name = "Hủy kích hoạt")]
        Deactive = 2
	}
}
