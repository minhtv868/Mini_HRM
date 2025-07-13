using Web.Application.Common.Mappings;
using Web.Domain.Entities.Jobs;

namespace Web.Application.Features.Finances.JobQueues.DTOs
{
    public class RemapPlayerCareerTranferInjuryDto
    {
        public string DataSouceName { get; set; }
        public List<int> Ids { get; set; }
    }
}
