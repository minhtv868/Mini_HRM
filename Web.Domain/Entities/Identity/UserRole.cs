using Web.Domain.Common;

namespace Web.Domain.Entities.Identity
{
    public class UserRole : BaseAuditableEntity
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }
}
