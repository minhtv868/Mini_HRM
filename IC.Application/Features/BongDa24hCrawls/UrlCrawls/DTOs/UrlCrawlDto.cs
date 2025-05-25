using IC.Application.Common.Mappings;
using IC.Domain.Entities.BongDa24hCrawls;

namespace IC.Application.Features.BongDa24hCrawls.UrlCrawls.DTOs
{
	public class UrlCrawlDto:IMapFrom<UrlCrawl>
	{
		public int Id { get; set; }
		public string Url { get; set; }
		public string UrlDesc { get; set; }
		public string UrlType { get; set; }
		public long? UrlHash { get; set; }
		public string UrlGroup { get; set; }
		public bool IsCrawled { get; set; }
		public string BatchCode { get; set; }
		public string CrawlResult { get; set; }
		public DateTime? CrawlTime { get; set; }
		public DateTime CrDateTime { get; set; }
	}
}
