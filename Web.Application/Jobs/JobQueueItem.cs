using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Application.Jobs
{
	public class JobQueueItem
	{
		public string JobQueue { get; set; } = "default";
		public string JobQueueDesc { get; set; } = "default";
	}
}
