using Web.Domain.Common;

namespace Web.Domain.Entities.Finance
{
    public class Bank : BaseAuditableEntity
    {
        public int BankId { get; set; }
        public string BankCode { get; set; } = string.Empty;
        public string BankName { get; set; } = string.Empty;
        public string LogoUrl { get; set; }
        public string Website { get; set; }
        public bool IsActive { get; set; } = true;
    }

}
