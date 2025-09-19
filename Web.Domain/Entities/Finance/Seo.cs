using Web.Domain.Common;

namespace Web.Domain.Entities.Finance
{
    public class Seo : BaseAuditableEntity
    {
        public int SeoId { get; set; }
        public short? SiteId { get; set; }
        public string SeoName { get; set; }
        public string Url { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDesc { get; set; }
        public string MetaKeyword { get; set; }
        public string CanonicalTag { get; set; }
        public string H1Tag { get; set; }
        public string SeoFooter { get; set; }
        public string ImageUrl { get; set; }
        public int? CrUserId { get; set; }
        public DateTime CrDateTime { get; set; }
        public int? UpdUserId { get; set; }
        public DateTime? UpdDateTime { get; set; }
    }
}