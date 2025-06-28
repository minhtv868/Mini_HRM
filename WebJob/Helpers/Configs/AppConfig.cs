using WebJob.Models;

namespace WebJob.Helpers.Configs
{
    public static class AppConfig
    {
        public static AppSettings AppSettings { get; set; } = new AppSettings();
        public static MailSettings MailSettings { get; set; } = new MailSettings();

        public static readonly string LinkSetAuthen2FA = "/Identity/Account/Manage/EnableAuthenticator";
        public static readonly string LinkChangePass = "/Identity/Account/Manage/ChangePassword";
		
	}
}
