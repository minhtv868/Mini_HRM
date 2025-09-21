namespace Web.Application.Jobs
{
    public class JobRunningHelper
    {
        public static bool JobQueueProcessJobRunning = false;
        public static bool ProcessCrawlPrimaryLeagueJob = false;
        public static bool ProcessCrawlTopTranferGetHtmlJobRunning = false;
        public static bool ProcessGetSchedulesJob = false;
        public static bool FSPlayerGetCrawUrlByPlayer = false;
        public static bool ProcessGetSchedulesFromTemporaryDataJob = false;
        public static bool FSPlayerCareerTranferInjuryCrawlJob = false;

        public static void ResetRunningStatus()
        {
            JobQueueProcessJobRunning = false;

            ProcessGetSchedulesJob = false;

            FSPlayerGetCrawUrlByPlayer = false;

            ProcessGetSchedulesFromTemporaryDataJob = false;

            FSPlayerCareerTranferInjuryCrawlJob = false;
            ProcessCrawlPrimaryLeagueJob = false;
        }
    }
}
