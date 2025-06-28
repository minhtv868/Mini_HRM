using System.ComponentModel.DataAnnotations;

namespace Web.Domain.Enums
{
    public enum SectionEnum
    {
        [Display(Name = "Tin tức")]
        News = 1,

        [Display(Name = "Kiến thức")]
        Knowledge = 4,

        [Display(Name = "Video")]
        Video = 2,

        [Display(Name = "Audio")]
        Audio = 3,

        //[Display(Name = "Tư vấn")]
        //Advise = 5,

        //[Display(Name = "Podcast")]
        //Podcast = 8,

        //[Display(Name = "Radio")]
        //Radio = 9
    }
}
