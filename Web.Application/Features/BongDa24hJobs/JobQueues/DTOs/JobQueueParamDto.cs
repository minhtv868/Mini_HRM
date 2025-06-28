using Web.Application.Common.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.Features.BongDa24hJobs.JobQueues.DTOs
{
    public class JobQueueParamDto : IMapFrom<JobQueueDto>
	{
		public string DataAction { get; set; }
		public string DataId { get; set; }
		public string DataJson { get; set; }
	}
}
