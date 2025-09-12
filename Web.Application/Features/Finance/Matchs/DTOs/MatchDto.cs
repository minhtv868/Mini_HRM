using Web.Application.Common.Mappings;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Matchs.DTOs
{
    public class MatchDto : IMapFrom<Match>
    {
        public int MatchId { get; set; }
        public DateTime? EstimateStartTime { get; set; }
        public short? HomeId { get; set; }
        public short? AwayId { get; set; }
        public short? LeagueId { get; set; }
        public string HomeName { get; set; }
        public string AwayName { get; set; }
        public string HomeLogoPath { get; set; }
        public string AwayLogoPath { get; set; }
        public byte? HomeGoals { get; set; }
        public byte? AwayGoals { get; set; }
        public string StadiumName { get; set; }
        public string LeagueName { get; set; }
        public string LeagueImage { get; set; }
        public bool? IsLive { get; set; }
        public bool? IsHot { get; set; }
        public int? SiteId { get; set; }
        public int? CrUserId { get; set; }
        public DateTime CrDateTime { get; set; }
        public int? UpdUserId { get; set; }
        public DateTime? UpdDateTime { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}
