using Web.Domain.Common;

namespace Web.Domain.Entities.Jobs
{
    public class Job : BaseAuditableEntity
    {
        public int Id { get; set; }
        public string JobName { get; set; }
        public string JobClassName { get; set; }
        public string JobClassType { get; set; }
        public int? CrUserId { get; set; }
        public DateTime? CrDateTime { get; set; }
    }
}
