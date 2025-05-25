using IC.Domain.Common;
using System;

namespace IC.Domain.Entities.BongDa24hJobs
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
