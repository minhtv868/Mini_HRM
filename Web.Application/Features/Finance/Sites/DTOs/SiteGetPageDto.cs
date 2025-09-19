using Web.Application.DTOs;
using Web.Application.Interfaces;

namespace Web.Application.Features.Finance.Sites.DTOs
{
    public class SiteGetPageDto : SiteDto, IAuditable
    {
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public AuditableInfoDto AuditableInfo { get; set; }
    }
}
