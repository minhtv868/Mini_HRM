using Web.Domain.Common;

namespace Web.Domain.Entities.Finance
{
    public class Team : BaseAuditableEntity
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
        // Thông tin đội cụ thể
        public short? LeagueId { get; set; }
        public short? RootTeamId { get; set; }
        public short? StadiumId { get; set; }
        public byte? IsMainTeam { get; set; }   // 1 = chính, 0 = phụ
        public byte? StatusId { get; set; }
        public short? SortOrder { get; set; }
        public string TeamUrl { get; set; }
        public byte? TeamTypeId { get; set; }
        // Captain & Coach
        public string CaptainName { get; set; }
        public int? CaptainArticleId { get; set; }
        public string CoachName { get; set; }
        public int? CoachArticleId { get; set; }

        public int? SiteId { get; set; }
        public int? CrUserId { get; set; }
        public DateTime? CrDateTime { get; set; }
        public int? UpdUserId { get; set; }
        public DateTime? UpdDateTime { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}