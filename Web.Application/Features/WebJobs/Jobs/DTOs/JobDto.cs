using Web.Application.Common.Mappings;
using Web.Domain.Entities.Jobs; 

namespace Web.Application.Features.Finances.Jobs.DTOs
{
	public class JobDto:IMapFrom<Job>
	{
		public int Id { get; set; }
		public string JobName { get; set; }
		public string JobClassName { get; set; }
		public string JobClassType { get; set; }
		public int? CrUserId { get; set; }
		public DateTime? CrDateTime { get; set; }
	}
}
