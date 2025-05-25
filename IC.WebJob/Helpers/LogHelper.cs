using Serilog;

namespace IC.WebJob.Helpers
{
    public class LogHelper
    {
        private static readonly string filePath = "Logs/log-.txt";
        private static readonly string filePathErr = "Logs/errlog-.txt";
        private static readonly string filePathSql = "Logs/sqllog-.txt";

        public static void WriteError(string logContent)
        {
            //using serilog
            using (var log = new LoggerConfiguration()
                                    .WriteTo.File(filePathErr, rollingInterval: RollingInterval.Day)
                                    .CreateLogger())
            {
                log.Error(logContent);
            }
        }

        public static void LogSql(string logContent)
        {
            //using serilog
            using (var log = new LoggerConfiguration()
                                    .WriteTo.File(filePathSql, rollingInterval: RollingInterval.Day)
                                    .CreateLogger())
            {
                log.Information(logContent);
            }
        }

        public static void WriteLog(string logContent)
        {
            //using serilog
            using (var log = new LoggerConfiguration()
                                    .WriteTo.File(filePath, rollingInterval: RollingInterval.Day)
                                    .CreateLogger())
            {
                log.Information(logContent);
            }
        }

        public static void WriteLog(string logContent, string methodName)
        {
            //using serilog
            using (var log = new LoggerConfiguration()
                                    .WriteTo.File(filePath, rollingInterval: RollingInterval.Day)
                                    .CreateLogger())
            {
                log.Information(methodName);
                log.Information(logContent);
            }
        }
    }
}
