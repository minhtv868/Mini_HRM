using Web.Application.Common.Helpers;

namespace Web.Application.Settings
{
    public class AppSettings
    {
        public const string SectionName = nameof(AppSettings);
        public int PageSize { get; set; }
        public string LuatVietnamGetDataDictionaryAPI { get; set; }
        public string FileImageAllow { get; set; } = ".jpeg,.jpg,.png,.gif";
        public string FileExtentionAllow { get; set; } = "jpeg,jpg,png,gif,mp4,mp3";
        public string FileWordExtentionAllow { get; set; } = "doc,docx";
        public int MaxWidthUpload { get; set; } = 4000;
        public int MaxHeightUpload { get; set; } = 4000;
        public bool CacheSysMenu { get; set; } = false;
        public bool RequireChangePassFirstTime { get; set; } = false;
        public bool RequireChangePassSchedule { get; set; } = false;
        public int MonthChangePassSchedule { get; set; } = 12;
        public bool ExternalLogin { get; set; } = true;
        public bool RequireAuth2Fa { get; set; } = false;
        public string IpWhiteList { get; set; }
        public string RootPath { get; set; } = "/";
        public string CmsDomain { get; set; }
        public string CmsDomainApi { get; set; }
        public string MediaDomain { get; set; }
        public string VideoDomain { get; set; } = "";
        public string NoImage { get; set; }
        public string WebsiteDomain { get; set; }
        public string UploadRootDir { get; set; }
        public string UploadDir { get; set; } = "Uploads";
        public bool SyncFileEnable { get; set; } = true;
        public string SyncFileHost { get; set; } = "127.0.0.1";
        public int SyncFilePort { get; set; } = 8688;
        public bool UsingDirByFileType { get; set; } = true;
        public byte SyncStatusIdDefault { get; set; } = 1;
        public bool IgnoreUploadDirInUrlFilePath { get; set; } = false;
        public bool UsingUploadApi { get; set; } = true;
        public string UploadApiUrl { get; set; }
        public string CopyFileApiUrl { get; set; }
        public string UploadApiKey { get; set; }
        public string ImageWaterMark { get; set; }
        public List<ThumbImageSettings> ThumbImageSettings { get; set; }
        public FacebookImageSettings FacebookImageSettings { get; set; }
        public AmpImageSettings AmpImageSettings { get; set; }
        #region Elastic
        public string ConnectionStringElastic { get; set; }
        public string ElasticIndexName { get; set; }
        public string UserElastic { get; set; }
        public string PassElastic { get; set; }
        public List<string> ListDomains = new List<string>
          {
        ""
          };
        public string PostDataApiKey { get; set; }
        public string WHOSCORE_COOKIES { get; set; } = "_ga=GA1.2.1522980637.1638238274; _gid=GA1.2.1608048762.1638238274; _fbp=fb.1.1638238275487.601814135; _pbjs_userid_consent_data=6683316680106290; _pubcid=771f9e5f-9707-4b00-b58e-bdcc08a56348; _xpid=3332333905; _xpkey=AVNP2VWOE7vCxdhFmBuShSFOF1FkKxOM; __qca=P0-1000656407-1638238275987; _lr_env_src_ats=false; _unifiedid=%7B%22TDID%22%3A%22801a42c3-661b-4a1d-8fa5-534d927ada4d%22%2C%22TDID_LOOKUP%22%3A%22TRUE%22%2C%22TDID_CREATED_AT%22%3A%222021-10-30T02%3A11%3A18%22%7D; _cc_id=3872495c44b939128bae8da5ea8c5003; visid_incap_774904=EiT0yfmpTeGrVPtBfix0f12SpWEAAAAAQUIPAAAAAADjf37CXxUIpj2ZCwGLYdyM; __gads=ID=54286bf39144672c:T=1638241904:S=ALNI_MbvUgosk2E_OXJrTZOxxnQroaYudg; visid_incap_774906=TxEWYgBsTlKhrIvlkX0SLz+IpWEAAAAAQ0IPAAAAAACAxqegAWcD/iYq/JPTUAhO3L4wqaKmRMxJ; incap_ses_1047_774906=tbzUWuGG4zRFnsELX7GHDkArqGEAAAAAbD5ZM3+O6HpNeRlMiWbSgA==; _gat=1; _gat_subdomainTracker=1; _lr_retry_request=true; panoramaId_expiry=1639015908027; panoramaId=34b53f0597abb51df35567c611cb4945a7022e0cab29cc6874edf072d9968707; cto_bidid=iZynil9xUTh3cU5HQzZpNWttRHlkYWp3TXgzUFBYVm9pME90cm5sbmVQJTJCb20lMkYlMkI5MXkyYzdUWktlUWtUZ3FmeFVWR29idSUyQnN0eHJlWHc2SVZQbXBDem81OTE5UjNvOVRMbSUyQnZSV1lJZk5zY3VEUmMlM0Q; cto_bundle=AtxXdV8lMkJ1MklZc1hMcFglMkZIdDIyRjd4cXBhR25SZnBSeGNDZW5KQWtibHRBVXZJZ2M4eUFCRCUyQmlucGloR0YwYXBsVFVjSUN6ZHFseFBTWEJ6bHVqTVBlNU5MTHdiRSUyQkhSa1BraFlycFZLQkFTMUJTNzM5WW5uUGhvNktIUHhIcjlidTZ1N3J6eENYUDh4RThtM0UlMkJZNE1yTzhBJTNEJTNE";
        public string FIFA_RANKING_MEN_URL { get; set; }
        public string FIFA_RANKING_WOMEN_URL { get; set; }
        public string FIFA_RANKING_MEN_DATA_URL { get; set; }
        public string FIFA_RANKING_WOMEN_DATA_URL { get; set; }

        #endregion
    }
}
