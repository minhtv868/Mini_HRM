using Web.Domain.Common;

namespace Web.Domain.Entities.Finance
{
    public class Department : BaseAuditableEntity
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentDesc { get; set; }
        public int? ManagerId { get; set; }
        public int? SiteId { get; set; }
        public int? CrUserId { get; set; }
        public DateTime? CrDateTime { get; set; }
        public int? UpdUserId { get; set; }
        public DateTime? UpdDateTime { get; set; }
    }
}