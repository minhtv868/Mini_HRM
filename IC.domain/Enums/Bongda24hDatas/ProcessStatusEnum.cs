using System.ComponentModel.DataAnnotations;

namespace IC.Domain.Enums.Bongda
{
	public enum ProcessStatusEnum
	{
		[Display(Name = "Chờ Crawl")]
		WaitCrawl = 1,

		[Display(Name = "Đang Crawl")]
		Crawling = 2,

		[Display(Name = "Crawl lỗi")]
		CrawlError = 3,

		[Display(Name = "Chờ Parse")]
		WaitParse = 4,

		[Display(Name = "Đang Parse")]
		Parsing = 5,

		[Display(Name = "Parse lỗi")]
		ParseError = 6,

		[Display(Name = "Hoàn thành")]
		Done = 7
	}
}
