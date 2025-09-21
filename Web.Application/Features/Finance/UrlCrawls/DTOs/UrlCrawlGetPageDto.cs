using Web.Application.DTOs;
using Web.Application.Interfaces;

namespace Web.Application.Features.Finance.UrlCrawls.DTOs
{
    public class UrlCrawlGetPageDto : UrlCrawlDto, IAuditable
    {
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public AuditableInfoDto AuditableInfo { get; set; }
        public int? CrUserId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int? UpdUserId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
