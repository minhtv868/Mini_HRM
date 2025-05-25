using IC.Domain.Common;

namespace IC.Domain.Entities.Identity
{
    public class SysFunctionRole : BaseAuditableEntity
    {
        public int SysFunctionId { get; set; }
        public int RoleId { get; set; }
    }
}
