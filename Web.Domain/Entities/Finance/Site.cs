using Web.Domain.Common;

namespace Web.Domain.Entities.Finance
{
    public class Site : BaseAuditableEntity
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; }
        public string ShortName { get; set; }
        public string WebsiteDomain { get; set; }
        public string Logo { get; set; }
        public bool IsActive { get; set; } = true;
        public short? DisplayOrder { get; set; }
        public int? CrUserId { get; set; }
        public DateTime CrDateTime { get; set; }
        public int? UpdUserId { get; set; }
        public DateTime? UpdDateTime { get; set; }
    }
}