using IC.Application.Common.Mappings;
using IC.Domain.Entities.BongDa24hJobs; 

namespace IC.Application.Features.BongDa24hJobs.Jobs.DTOs
{
    public class JobGetAllDto : IMapFrom<Job>
    {
        public int Id { get; set; }
        public string JobName { get; set; } 
    }
}
