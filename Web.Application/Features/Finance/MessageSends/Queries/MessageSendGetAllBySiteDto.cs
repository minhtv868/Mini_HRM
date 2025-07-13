using Web.Application.Common.Mappings;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.MessageSends.DTOs
{
    public class MessageSendGetAllBySiteDto : IMapFrom<MessageSend>
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
    }
}
