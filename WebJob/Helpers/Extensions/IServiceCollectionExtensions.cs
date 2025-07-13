using Hangfire;
using Hangfire.MediatR;
using Web.Application.Common.Jobs;
using WebJob.Helpers.Configs;

namespace WebJob.Helpers.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddBindAppConfig(this IServiceCollection services, IConfiguration configuration)
        {
            configuration.GetSection("AppSettings").Bind(AppConfig.AppSettings);
            configuration.GetSection("MailSettings").Bind(AppConfig.MailSettings);
        }

        public static void AddHangfireService(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null) return;

            GlobalJobFilters.Filters.Add(new PreventConcurrentExecutionJobFilter());

            //Hangfire
            services.AddHangfire(hf =>
            {
                hf.UseSqlServerStorage(configuration.GetConnectionString("FinanceConnection"));
                hf.UseMediatR();
            });
            var hangfireQueues = new[] { "process_cache", "process_data", "process_log", "default" };
            var hangfireServerName = Environment.MachineName;
            string queueConfigs = configuration.GetSection("AppSettings").GetValue<string>("HangfireQueues");
            string serverNameConfigs = configuration.GetSection("AppSettings").GetValue<string>("HangfireServerName");
            if (!string.IsNullOrEmpty(queueConfigs))
            {
                hangfireQueues = queueConfigs.Split(",");
            }
            if (!string.IsNullOrEmpty(serverNameConfigs))
            {
                hangfireServerName = serverNameConfigs + "-" + hangfireServerName;
            }
            services.AddHangfireServer(option =>
            {
                option.SchedulePollingInterval = TimeSpan.FromSeconds(5);
                option.ServerName = hangfireServerName;
                option.Queues = hangfireQueues;
            });
        }
    }
}
