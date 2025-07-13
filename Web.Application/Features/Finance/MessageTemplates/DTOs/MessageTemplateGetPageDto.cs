using Web.Application.DTOs;
using Web.Application.Interfaces;

namespace Web.Application.Features.Finance.MessageTemplates.DTOs
{
    public class MessageTemplateGetPageDto : MessageTemplateDto, IAuditable
    {
        public string SendMethod { get; set; }
        public string CrUser { get; set; }
        public int? UpdUserId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int? CreatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int? UpdatedBy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public AuditableInfoDto AuditableInfo { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
