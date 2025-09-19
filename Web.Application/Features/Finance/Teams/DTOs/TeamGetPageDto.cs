using Web.Application.DTOs;
using Web.Application.Interfaces;

namespace Web.Application.Features.Finance.Teams.DTOs
{
    public class TeamGetPageDto : TeamDto, IAuditable
    {
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public AuditableInfoDto AuditableInfo { get; set; }

    }
}
