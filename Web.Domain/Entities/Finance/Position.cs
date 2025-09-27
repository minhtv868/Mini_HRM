using Web.Domain.Common;

namespace Web.Domain.Entities.Finance
{
    public class Position : BaseAuditableEntity
    {
        public int PositionId { get; set; }
        public string PositionName { get; set; }
        public string Description { get; set; }
        public int? ManagerId { get; set; }
        public int? CrUserId { get; set; }
        public DateTime? CrDateTime { get; set; }
        public int? UpdUserId { get; set; }
        public DateTime? UpdDateTime { get; set; }
    }
}