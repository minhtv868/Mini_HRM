using IC.Domain.Common;

namespace IC.Domain.Entities.Identity
{
    public class UserRole : BaseAuditableEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
