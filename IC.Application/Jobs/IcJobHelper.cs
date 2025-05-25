using IC.Domain.Entities.BongDa24hJobs;

namespace IC.Application.Jobs
{
    public class IcJobHelper
    {
        private static List<JobQueueItem> jobQueues =
            [
				new JobQueueItem
                {
					JobQueue = "default",
					JobQueueDesc = "default" 
				},
				new JobQueueItem
				{
					JobQueue = "dev",
					JobQueueDesc = "dev"
				},
				new JobQueueItem
				{
					JobQueue = "beta",
					JobQueueDesc = "beta"
				},
				new JobQueueItem
				{
					JobQueue = "prod",
					JobQueueDesc = "prod"
				}
			]; 

        private static List<JobSchedule> jobSchedules = new List<JobSchedule>()
        {
            new JobSchedule()
            {
                JobScheduleName = "Chạy 1 lần",
                JobScheduleType = "OneTime",
            },
            new JobSchedule()
            {
                JobScheduleName = "Lặp lại theo giây",
                JobScheduleType = "RepeatSecond",
            },
            new JobSchedule()
            {
                JobScheduleName = "Lặp lại theo phút",
                JobScheduleType = "RepeatMinute",
            },
            new JobSchedule()
            {
                JobScheduleName = "Lặp lại theo giờ",
                JobScheduleType = "RepeatHour",
            },
            new JobSchedule()
            {
                JobScheduleName = "Lặp lại hàng ngày",
                JobScheduleType = "RepeatDay",
            },
        };
		public static List<JobQueueItem> GetJobQueueList()
		{
			return jobQueues.ToList();
		}

		public static List<JobSchedule> GetJobScheduleList()
        {
            return jobSchedules;
        }

        public static string GetJobSchedule(string jobScheduleType, int timeNumber)
        {
            if (jobScheduleType == "RepeatSecond")
            {
                return "*/" + timeNumber.ToString() + " * * * * *";
            }
            else if (jobScheduleType == "RepeatMinute")
            {
                return "*/" + timeNumber.ToString() + " * * * *";
            }
            else if (jobScheduleType == "RepeatHour")
            {
                return "0 */" + timeNumber.ToString() + " * * *";
            }
            else if (jobScheduleType == "RepeatDay")
            {
                return "0 " + (timeNumber - 7).ToString() + " * * *";
            }

            return "";
        }
    }
}
