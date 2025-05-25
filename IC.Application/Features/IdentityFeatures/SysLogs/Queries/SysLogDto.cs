using IC.Application.Common.Mappings;
using IC.Domain.Entities.Identity;

namespace IC.Application.Features.IdentityFeatures.SysLogs.Queries
{
	public class SysLogDto: IMapFrom<SysLog>
	{
		public int Id { get; set; }
		public string Message { get; set; }
		public string Level { get; set; }
		public DateTime? TimeStamp { get; set; }
		public string Exception { get; set; }
		public string SourceContext { get; set; }
		public string Application { get; set; }
	}
}
