using Web.Domain.Common;

namespace Web.Domain.Entities.Finance
{
    public class UserSite : BaseAuditableEntity
    {
        public int UserSiteId { get; set; }
        public int UserId { get; set; }
        public int SiteId { get; set; }
        public int? CrUserId { get; set; }
        public DateTime? CrDateTime { get; set; }

    }
}