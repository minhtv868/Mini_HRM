using Web.Domain.Common;

namespace Web.Domain.Entities.Identity
{
	public class SysFunctionUser : BaseAuditableEntity
	{
		public int SysFunctionId { get; set; }
		public int UserId { get; set; }
		public int? DisplayOrder { get; set; }
	}
}
