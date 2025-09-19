using Web.Application.Common.Mappings;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Teams.DTOs
{
    public class TeamGetAllDto : IMapFrom<Team>
    {
        public short TeamId { get; set; }
        public string TeamName { get; set; }
        public string TeamCode { get; set; }
        public string VNName { get; set; }
        public byte? NumberPlayer { get; set; }
        public short? CountryId { get; set; }
        public string Website { get; set; }
        public string LogoPath { get; set; }
        public string MainColor { get; set; }
        public string ContrastColor { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}
