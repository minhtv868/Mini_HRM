using Web.Domain.Common;

namespace Web.Domain.Entities.Finance
{
    public class UrlCrawl : BaseAuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public byte DataType { get; set; }
        public int? DataId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CrDateTime { get; set; }
    }
}
