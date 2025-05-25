using Hangfire;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Logging;
using Hangfire.Server;

namespace IC.Application.Common.Jobs
{
	public class PreventConcurrentExecutionJobFilter : JobFilterAttribute, IClientFilter, IServerFilter
	{
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        public void OnCreating(CreatingContext filterContext)
		{
			try
			{ 
                var jobs = JobStorage.Current.GetMonitoringApi().ProcessingJobs(0, 100);
                //Logger.Error(filterContext.Job.Type.ToString());
                //Logger.Error(filterContext.Job.Args.ToString());

                //if (jobs.Count(x => x.Value.Job.Type == filterContext.Job.Type) > 0)
                if (jobs != null && filterContext.Job != null && filterContext.Job.Args != null && filterContext.Job.Args.Count > 0 && jobs.Count(x => x.Value.Job.Args != null && x.Value.Job.Args.Count > 0 && x.Value.Job.Args[0] == filterContext.Job.Args[0]) > 0)
                {
                    filterContext.Canceled = true;
                }
            }
            catch(Exception ex)
			{
                Logger.Error(ex.ToString());
            }
        }

		public void OnPerformed(PerformedContext filterContext) { }

		void IClientFilter.OnCreated(CreatedContext filterContext) { }

		void IServerFilter.OnPerforming(PerformingContext filterContext) { }
	}
}
