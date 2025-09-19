using Web.Application.Common.Mappings;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Sites.DTOs
{
    public class SiteDto : IMapFrom<Site>
    {
        public short SiteId { get; set; }
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
