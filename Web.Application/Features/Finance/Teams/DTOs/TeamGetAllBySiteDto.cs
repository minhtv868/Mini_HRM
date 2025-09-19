using Web.Application.Common.Mappings;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.Teams.DTOs
{
    public class TeamGetAllBySiteDto : IMapFrom<Team>
    {
        public short TeamId { get; set; }
        public string TeamCode { get; set; }
        public string TeamNameSMS { get; set; }
        public string TeamAlias { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string TeamDesc { get; set; } = string.Empty;
        public string TeamImage { get; set; }
        public short? TeamOwnerId { get; set; }
        public byte? RelegationNum { get; set; }
        public short? SortOrder { get; set; }
        public string LivescoreNames { get; set; }
        public byte? StatusId { get; set; }
        public byte? TeamIndex { get; set; }
        public string TeamUrl { get; set; }
        public string TeamArea { get; set; }
        public string MainColor { get; set; }
        public string ContrastColor { get; set; }
        public DateTime? EstablishTime { get; set; }
        public string EstablishText { get; set; }
        public string TotalClub { get; set; }
        public string EliminationRoundFor { get; set; }
        public string TeamRelate { get; set; }
        public string CurrentChampionTeam { get; set; }
        public string BestTeam { get; set; }
        public string TelevisionList { get; set; }
        public string LinkFacebook { get; set; }
        public string LinkTwitter { get; set; }
        public string LinkYoutube { get; set; }
        public string LinkInstagram { get; set; }
        public string Website { get; set; }
        public int? VideoTagId { get; set; }
        public int? LiveTagId { get; set; }
        public int? BiographyArticleId { get; set; }
        public string HomeCountry { get; set; }
        public string StartToEnd { get; set; }
        public string PlayArea { get; set; }
        public string BannerImage { get; set; }
        public byte IsBXH { get; set; }
        public byte? DisplayType { get; set; }
        public byte? Featured { get; set; }
        public byte? TeamType { get; set; }
        public int? SiteId { get; set; }
        public int? CrUserId { get; set; }
        public DateTime CrDateTime { get; set; }
        public int? UpdUserId { get; set; }
        public DateTime? UpdDateTime { get; set; }
        public DateTime? LastUpdateTime { get; set; }
    }
}
