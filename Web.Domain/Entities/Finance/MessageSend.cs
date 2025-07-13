using Web.Domain.Common;

namespace Web.Domain.Entities.Finance
{
    public class MessageSend : BaseAuditableEntity
    {
        public int MessageSendId { get; set; }
        public short? SiteId { get; set; }
        public short MessageTemplateId { get; set; }
        public int? CampaignId { get; set; }
        public string SendFrom { get; set; }
        public string SendTo { get; set; }
        public string Title { get; set; }
        public string MsgContent { get; set; }
        public byte SendMethodId { get; set; }
        public byte SendStatusId { get; set; }
        public DateTime CrDateTime { get; set; }
        public DateTime? SendTime { get; set; }
        public DateTime? OpenMailTime { get; set; }
        public DateTime? ClickLinkTime { get; set; }
    }
}
