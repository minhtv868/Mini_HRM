using IC.Domain.Common;

namespace IC.Domain.Entities.Identity
{
	public class ApiKey : BaseAuditableEntity
	{
		public int Id { get; set; }
		public string PartnerName { get; set; }
		public string PartnerDesc { get; set; }
		public string AppName { get; set; }
		public string AppKey { get; set; }
		public string IpList { get; set; }
		public bool IsActive { get; set; }
		public int? CrUserId { get; set; }
		public DateTime? CrDateTime { get; set; }
    }
}
