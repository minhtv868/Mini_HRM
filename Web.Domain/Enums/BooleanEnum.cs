using System.ComponentModel.DataAnnotations;

namespace Web.Domain.Enums
{
    public enum BooleanEnum
    {
        [Display(Name = "Có")]
        Yes = 1,

        [Display(Name = "Không")]
        No = 2
    }
}
