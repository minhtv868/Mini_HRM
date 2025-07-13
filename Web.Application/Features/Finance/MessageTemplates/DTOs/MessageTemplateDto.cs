using Web.Application.Common.Mappings;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.MessageTemplates.DTOs
{
    public class MessageTemplateDto : IMapFrom<MessageTemplate>
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
