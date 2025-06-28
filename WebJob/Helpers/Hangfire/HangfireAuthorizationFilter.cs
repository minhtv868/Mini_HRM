using Hangfire.Dashboard;

namespace WebJob.Helpers.Hangfire
{
	public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
	{
		public bool Authorize(DashboardContext context)
		{
			var httpContext = context.GetHttpContext();

			// Allow all authenticated users to see the Dashboard (potentially dangerous).
			return httpContext.User.IsInRole("Admin") || httpContext.User.IsInRole("Super Admin");

			//return httpContext.User.Identity?.IsAuthenticated ?? false;
		}
	}
}
