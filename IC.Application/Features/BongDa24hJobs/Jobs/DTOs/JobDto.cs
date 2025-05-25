using IC.Application.Common.Mappings;
using IC.Domain.Entities.BongDa24hJobs; 

namespace IC.Application.Features.BongDa24hJobs.Jobs.DTOs
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
