namespace Web.Shared.Helpers
{
	public class SlugHelper
	{
		public static string CreateProfileSlug(int profileId, string profileName, DateTime? crDateTime)
		{
			return CreateSlug(SlugPrefixType.Profile, SlugContentType.Profile, "", profileName, profileId, crDateTime);
		}

        public static string CreateRadioTopicSpotlightSlug(int topicId, string slugTopicName, DateTime? crDateTime)
        {
            return CreateSlug(SlugPrefixType.RadioSection, SlugContentType.Spotlight, "", slugTopicName, topicId, crDateTime);
        }

        public static string CreateSlug(
			string slugPrefixType,
			string slugContentType,
			string cateTitle,
			string dataTitle,
			int dataId,
			DateTime? crDateTime
		)
		{
			if(crDateTime == null) { crDateTime = DateTime.Now; }

			var idUrl = slugContentType + crDateTime.ToStringFormat("yyMMdd") + dataId.ToString("000000000");
			var slug = string.Empty;

			if (!string.IsNullOrEmpty(slugPrefixType))
			{
				if (!string.IsNullOrEmpty(slug)) slug = slug + "/";
				slug = slug + slugPrefixType;
			}

			if (!string.IsNullOrEmpty(cateTitle))
			{
				cateTitle = cateTitle.Trim();
				cateTitle = StringHelper.RemoveSign4VietnameseString(cateTitle);
				cateTitle = StringHelper.RemoveSpecialCharInURL(cateTitle).ToLower().Trim();

				if (!string.IsNullOrEmpty(slug)) slug = slug + "/";

				slug = slug + cateTitle;
			}

			if (!string.IsNullOrEmpty(dataTitle))
			{
				dataTitle = dataTitle.Trim();
				dataTitle = StringHelper.RemoveSign4VietnameseString(dataTitle);
				dataTitle = StringHelper.RemoveSpecialCharInURL(dataTitle).ToLower().Trim();

				if (!string.IsNullOrEmpty(slug)) slug = slug + "/";

				slug = slug + dataTitle;
			}
			
			slug = "/" + slug + "-" + idUrl + ".html";

			return slug;
		}

		public static string CreateSlugOld(string dataTitle)
		{
			var slug = "";
			if (!string.IsNullOrEmpty(dataTitle))
			{
				slug = StringHelper.RemoveSign4VietnameseString(dataTitle);
				slug = StringHelper.RemoveSpecialCharInURL(slug).ToLower().Trim();
			}
			
			return slug;
		}

		public class SlugPrefixType
		{
			public static readonly string Info = "";
			public static readonly string Tag = "tag";
			public static readonly string Profile = "profile";
			public static readonly string NewsSection = "tin-tuc";
			public static readonly string KnowledgeSection = "kien-thuc";
			public static readonly string RadioSection = "radio";
			public static readonly string PodCastSection = "podcast";
			public static readonly string VideoSection = "video";
			public static readonly string EventSection = "su-kien";
			public static readonly string AdviseSection = "tu-van";

		}
		public class SlugContentType
		{
			public static readonly string Info = "00";
			public static readonly string Section = "01";
			public static readonly string Cate = "02";
			public static readonly string Tag = "03";
			public static readonly string Spotlight = "05";
			public static readonly string Profile = "04";
			public static readonly string Article = "11";
			public static readonly string Education = "12";
			public static readonly string Podcast = "13";
			public static readonly string Radio = "14";
			public static readonly string Video = "15";
			public static readonly string VideoShort = "16";
		}
	}
}
