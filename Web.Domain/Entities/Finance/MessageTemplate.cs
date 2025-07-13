using Web.Domain.Common;

namespace Web.Domain.Entities.Finance
{
    public class MessageTemplate : BaseAuditableEntity
    {
        public short MessageTemplateId { get; set; }
        public short? SiteId { get; set; }
        public string MessageName { get; set; }
        public string SendFrom { get; set; }
        public string Title { get; set; }
        public string MsgContent { get; set; }
        public byte SendMethodId { get; set; }
        public int? CrUserId { get; set; }
        public DateTime? CrDateTime { get; set; }
    }
}
