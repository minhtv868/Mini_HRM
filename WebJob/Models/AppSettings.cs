namespace WebJob.Models
{
	public class AppSettings
    {
        public bool UseMapToId { get; set; } = false;
        public double CacheTimeExpireInMinute { get; set; } = 10;
        public int PageSize { get; set; } = 30;
        public string FileExtentionAllow { get; set; } = "jpeg,jpg,png,gif";
        public bool CacheSysMenu { get; set; } = false;
        public bool RequireChangePassFirstTime { get; set; } = false;
        public bool RequireAuth2Fa { get; set; } = false;
        public string TwoFactorAuthenticatorAppTitle { get; set; } = "IC CMS";
        public string IpWhiteList { get; set; } = "";
        public string CmsDomain { get; set; }
        public string MediaDomain { get; set; }
        public string NoImage { get; set; }
        public string WebsiteDomain { get; set; }
        public string UploadRootDir { get; set; }
        public string UploadDir { get; set; } = "Uploads";
		public string NaviCDNDomain { get; set; } = "https://dev.navicdn.com/";
		public bool IgnoreUploadDirInUrlFilePath { get; set; } = false;
        public bool UsingUploadApi { get; set; } = false;
        public string UploadApiUrl { get; set; }
        public string CopyFileApiUrl { get; set; }
        public string UploadApiKey { get; set; }
		public string ErrorMessage { get; set; } = "Vui lòng thử lại sau.";
		public string RouteNameBongDa24hCms { get; set; } = "BongDa24hCMS";
		//public List<ThumbImageSettings> ThumbImageSettings { get; set; }
		//public FacebookImageSettings FacebookImageSettings { get; set; }
		//public AmpImageSettings AmpImageSettings { get; set; }
	}
}
