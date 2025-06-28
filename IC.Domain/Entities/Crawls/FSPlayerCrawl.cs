using IC.Domain.Common;

namespace IC.Domain.Entities.Crawls
{
    public class FSPlayerCrawl : BaseAuditableEntity
    {
        public int Id { get; set; }
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string FSPlayerId { get; set; }
        public string FSPlayerUrl { get; set; }
        public string UrlCrawl { get; set; }
        public long? UrlHash { get; set; } // Được tạo bởi PlayerId, UrlCrawl
        public long? ContentHash { get; set; } // Được tạo bởi UrlCrawl, Career, Transfers, InjuryHistory
        public string Career { get; set; }
        public string Transfers { get; set; }
        public string InjuryHistory { get; set; }
        public byte ProcessStatusId { get; set; } // Trạng thái để xác định crawl hay parse, 1: Chờ crawl, 2: Crawl lỗi, 3: Chờ Parse, 4: Parse lỗi, 5: Thành công
        public string BatchCode { get; set; }
        public string ProcessResult { get; set; }
        public DateTime? ProcessTime { get; set; }
        public DateTime CrDateTime { get; set; }
    }
}
