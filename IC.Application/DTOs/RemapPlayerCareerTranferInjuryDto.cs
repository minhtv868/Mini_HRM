using IC.Application.Common.Mappings;
using IC.Domain.Entities.BongDa24hJobs;

namespace IC.Application.Features.BongDa24hJobs.JobQueues.DTOs
{
    public class RemapPlayerCareerTranferInjuryDto
    {
        public string DataSouceName { get; set; }
        public List<int> Ids { get; set; }
    }
}
