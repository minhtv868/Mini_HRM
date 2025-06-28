using Web.Application.Common.Mappings;
using Web.Domain.Entities.Jobs;

namespace Web.Application.Features.BongDa24hJobs.JobQueues.DTOs
{
    public class JobQueueGetPageDto : IMapFrom<JobQueue>
    {
        public int Id { get; set; }
        public string DataSouceName { get; set; }
        public string DataId { get; set; }
        public string DataJson { get; set; }
        public string JobName { get; set; }
        public long? Hash { get; set; }
        public string BatchCode { get; set; }
        public int? CrUserId { get; set; }
        public DateTime? CrDateTime { get; set; }
    }
}
