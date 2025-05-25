using IC.Domain.Common;

namespace IC.Domain.Entities.Identity
{
	public class SysFunctionUser : BaseAuditableEntity
	{
		public int SysFunctionId { get; set; }
		public int UserId { get; set; }
		public int? DisplayOrder { get; set; }
	}
}
