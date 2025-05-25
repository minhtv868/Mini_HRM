namespace IC.WebJob.Models
{
	public class QueryInput
	{
		public bool IsHot { get; set; } = false;
		public bool IsMostViewed { get; set; } = false;
		public bool IsLive { get; set; } = false;
		public int DayNumber { get; set; }
		public int NewsTypeId { get; set; } = 1;
		public int DayNumberMostView { get; set; }
	}
}
