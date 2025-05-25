using IC.Application.Common.Mappings;
using IC.Domain.Entities.BongDa24hCrawls; 

namespace IC.Application.Features.BongDa24hCrawls.UrlCrawls.DTOs
{
	public class UrlCrawlJsonDto : IMapFrom<UrlCrawl>, IMapFrom<UrlCrawlDto>
	{
		public int Id { get; set; }
		public string Url { get; set; } 
		public string UrlType { get; set; } 
		public string UrlGroup { get; set; } 
	}
}
