using Web.Domain.Common;

namespace Web.Domain.Entities.Jobs
{
    public class JobQueue : BaseAuditableEntity
    {
        public int Id { get; set; }
        public string DataSouceName { get; set; }
        public string DataId { get; set; }
        public string DataJson { get; set; }
        public string JobName { get; set; }
        public bool IsPublicJob { get; set; }
        public long? Hash { get; set; }
        public string BatchCode { get; set; }
        public int? CrUserId { get; set; }
        public DateTime? CrDateTime { get; set; }
    }
}
