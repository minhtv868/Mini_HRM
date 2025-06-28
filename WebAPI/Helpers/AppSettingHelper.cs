namespace WebAPI.Helpers
{
    public class AppSettingHelper
    {
        private static AppSettingHelper _appSettings;
        public string AppSettingValue { get; set; }
        public static string AppSetting(string key)
        {
            _appSettings = GetCurrentSettings(key);
            return _appSettings.AppSettingValue;
        }

        public AppSettingHelper(IConfiguration config, string key)
        {
            this.AppSettingValue = config.GetValue<string>(key);
        }

        public static AppSettingHelper GetCurrentSettings(string key)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();

            var settings = new AppSettingHelper(configuration.GetSection("AppSettings"), key);

            return settings;
        }
        public static string RootPath = !string.Equals(AppSetting("ROOT_PATH"), null, StringComparison.Ordinal) ? AppSetting("ROOT_PATH") : "/";
        public static string LVNDocGroupId = !string.Equals(AppSetting("LVNDocGroupId"), null, StringComparison.Ordinal) ? AppSetting("LVNDocGroupId") : ",1,2,3,4,5,6,";
        public static string InforceEffectStatusId = !string.Equals(AppSetting("InforceEffectStatusId"), null, StringComparison.Ordinal) ? AppSetting("InforceEffectStatusId") : ",2,3,4,";




    }
}
