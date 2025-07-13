using Web.Application.Common.Mappings;
using Web.Domain.Entities.Jobs; 

namespace Web.Application.Features.Finances.Jobs.DTOs
{
    public class JobGetAllDto : IMapFrom<Job>
    {
        public int Id { get; set; }
        public string JobName { get; set; } 
    }
}
