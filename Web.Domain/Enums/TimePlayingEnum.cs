using System.ComponentModel.DataAnnotations;

namespace Web.Domain.Enums
{
    public enum TimePlayingEnum
    {

        [Display(Name = "Kết thúc")]
        FT = 1,
        [Display(Name = "Chưa đá")]
        NS = 2,
    }

}
