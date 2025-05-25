using System.ComponentModel.DataAnnotations;

namespace IC.Domain.Enums
{
	public class DataTypeSeoEnum
	{
        [Display(Name = "Seo chủ đề")]
        public static readonly string CategoryTable = "CategoryTable";

        [Display(Name = "Seo bản án")]
        public static readonly string LawJudgTable = "LawJudgTable";

        [Display(Name = "Seo tag")]
        public static readonly string TagTable = "TagTable";

        [Display(Name = "Seo theo url")]
        public static readonly string LawJudgByUrl = "LawJudgByUrl";

		[Display(Name = "Seo theo template")]
		public static readonly string LawJudgByTemplate = "LawJudgByTemplate";
        
        [Display(Name = "Thông tin Seo")]
		public static readonly string SeoSocialInfo = "SeoSocialInfo";

	}
}
