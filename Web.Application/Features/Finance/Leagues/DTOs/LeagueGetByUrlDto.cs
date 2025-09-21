using Web.Application.Common.Mappings;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Leagues.DTOs
{
    public class LeagueGetByUrlDto : IMapFrom<League>
    {
        public short LeagueId { get; set; }
        public string LeagueName { get; set; } = string.Empty;
        public byte? StatusId { get; set; }
        public byte? LeagueIndex { get; set; }
        public string LeagueUrl { get; set; }
    }
}
