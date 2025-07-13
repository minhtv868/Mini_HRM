using Web.Application.Features.Finance.MessageSends.DTOs;

namespace Web.Application.Features.Finance.MessageSends.Queries
{
    public class MessageSendGetPageDto : MessageSendDto
    {
        public string SendMethodName { get; set; }
        public string SendStatusName { get; set; }
    }
}
