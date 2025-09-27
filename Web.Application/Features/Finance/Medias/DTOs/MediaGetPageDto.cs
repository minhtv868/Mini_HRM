using Web.Application.DTOs;
using Web.Application.Interfaces;

namespace Web.Application.Features.Finance.Medias.DTOs
{
    public class MediaGetPageDto : MediaDto, IAuditable
    {
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public AuditableInfoDto AuditableInfo { get; set; }
    }
}
