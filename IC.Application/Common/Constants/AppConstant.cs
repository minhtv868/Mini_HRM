using Microsoft.Extensions.Configuration;
using Nest;

namespace IC.Application.Common.Constants
{
    public static class AppConstant
    {
        public static int TIME_CACHE_VIEW { get; private set; }
        public static string PREFIX_LOCAL { get; private set; }
        public static string Application { get; private set; }
        public static string MediaDomain { get; private set; }
        public static string WebsiteDomain { get; private set; }
        public static string DIFF_UNIQID_ADS { get; private set; }
        public static string NOTIFY_MAINTENACE_CMS_KEY { get; private set; }

        public static string MAIL_SERVER_HOST { get; private set; }
        public static string MAIL_USER { get; private set; }
        public static string MAIL_PASS { get; private set; }
        public static string MAIL_DOMAIN { get; private set; }
        public static string MAIL_URLBASE { get; private set; }
        public static string AWSSDK_ID { get; private set; }
        public static string AWSSDK_PASS { get; private set; }

        public static bool USING_AWS { get; private set; }
        public static bool USING_AWSSDK { get; private set; }

        public static string AWS_MAIL_USER { get; private set; }
        public static string AWS_MAIL_PASS { get; private set; }
        public static string AWS_MAIL_SERVER_HOST { get; private set; }

        public static List<string> DbConnections { get; private set; }
        public static int timeDelayCrawl = 5000;
		public static bool ISDEV { get; private set; }
		public static void setIsDev(bool isDev)
		{
			ISDEV = isDev;
		}

		public static void Init(IConfiguration configuration)
        {
            NOTIFY_MAINTENACE_CMS_KEY = "NOTIFY_MAINTENACE_CMS_KEY";
            PREFIX_LOCAL = configuration.GetValue<string>("AppSettings:PREFIX_LOCAL") ?? "/icads/";
            DIFF_UNIQID_ADS = configuration.GetValue<string>("AppSettings:DIFF_UNIQID_ADS") ?? "icads";
            TIME_CACHE_VIEW = int.Parse(configuration.GetValue<string>("AppSettings:TIME_CACHE_VIEW") ?? "60");
            MediaDomain = configuration.GetValue<string>("AppSettings:MediaDomain") ?? "";
            WebsiteDomain = configuration.GetValue<string>("AppSettings:WebsiteDomain") ?? "";
            Application = configuration.GetValue<string>("Serilog:Properties:Application") ?? "";

            MAIL_SERVER_HOST = configuration.GetValue<string>("AppSettings:MAIL_SERVER_HOST") ?? "";
            MAIL_USER = configuration.GetValue<string>("AppSettings:MAIL_USER") ?? "";
            MAIL_PASS = configuration.GetValue<string>("AppSettings:MAIL_PASS") ?? "";
            MAIL_DOMAIN = configuration.GetValue<string>("AppSettings:MAIL_DOMAIN") ?? "";
            MAIL_URLBASE = configuration.GetValue<string>("AppSettings:MAIL_URLBASE") ?? "";
            AWSSDK_ID = configuration.GetValue<string>("AppSettings:AWSSDK_ID") ?? "";
            AWSSDK_PASS = configuration.GetValue<string>("AppSettings:AWSSDK_PASS") ?? "";

            USING_AWS = configuration.GetValue<bool>("AppSettings:USING_AWS");
            USING_AWSSDK = configuration.GetValue<bool>("AppSettings:USING_AWSSDK");

            AWS_MAIL_USER = configuration.GetValue<string>("AppSettings:AWS_MAIL_USER") ?? "";
            AWS_MAIL_PASS = configuration.GetValue<string>("AppSettings:AWS_MAIL_PASS") ?? "";
            AWS_MAIL_SERVER_HOST = configuration.GetValue<string>("AppSettings:AWS_MAIL_SERVER_HOST") ?? "";

            DbConnections = configuration.GetSection("ConnectionStrings").GetChildren().OrderBy(x => x.Key).Select(c => c.Key).ToList();

        }
    }
}
