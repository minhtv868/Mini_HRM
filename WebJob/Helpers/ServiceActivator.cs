namespace WebJob.Helpers
{
    public class ServiceActivator
    {
        internal static IServiceProvider _serviceProvider = null;

        /// <summary>
        /// Configure ServiceActivator with full serviceProvider
        /// </summary>
        /// <param name="serviceProvider"></param>
        public static void Configure(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Create a scope where use this ServiceActivator
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public static IServiceScope GetScope(IServiceProvider serviceProvider = null)
        {
            var provider = serviceProvider ?? _serviceProvider;
            return provider?
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
        }

        //Sample usage
        //IConfiguration configuration = default;
        //var logDebugEf = false;
        //using (var serviceScope = ServiceActivator.GetScope())
        //{
        //    if (serviceScope != null)
        //    {
        //        configuration = (IConfiguration)serviceScope.ServiceProvider.GetService(typeof(IConfiguration));
        //        if (configuration != null)
        //        {
        //            if (configuration["LogEntityFrameworkQuery"] == "1") logDebugEf = true;
        //        }
        //    }
        //}
    }
}
