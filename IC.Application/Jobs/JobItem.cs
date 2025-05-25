namespace IC.Application.Jobs
{
	public class JobItem
	{
		public string JobName { get; set; }
		public string JobClassName { get; set; }
		public string JobClassType { get; set; }
		public string JobQueue { get; set; } = "default";
	}
}
