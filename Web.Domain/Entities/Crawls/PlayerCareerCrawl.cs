using Web.Domain.Common;
using System;

namespace Web.Domain.Entities.Crawls
{
	public class PlayerCareerCrawl : BaseAuditableEntity
	{
		public int Id { get; set; }
		public string UrlCrawl { get; set; }
		public int PlayerId { get; set; }
		public byte LeagueType { get; set; } // Loại giải đấu : VĐQG, Cúp Quốc Gia, Cúp Châu Lục, Đội Tuyển
		public string FootballSeasonName { get; set; }
		public string TeamName { get; set; }
		public string TeamLogo { get; set; }
		public string TeamUrl { get; set; }
		public string LeagueName { get; set; }
		public string LeagueLogo { get; set; }
		public string LeagueUrl { get; set; }
		public long? KeyHash { get; set; } // Bao gồm PlayerId, TeamName, LeagueName, FootballSeasonName
		public decimal? PlayerRating { get; set; }
		public int? MatchsPlayed { get; set; }
		public int? GoalsScored { get; set; }
		public int? Assists { get; set; }
		public int? YellowCards { get; set; }
		public int? RedCards { get; set; }
		public int Sort { get; set; }
		public long? ContentHash { get; set; } // Bao gồm PlayerId, PlayerRating, MatchsPlayed, GoalsScored, Assists, YellowCards, RedCards, Sort
		public DateTime CrDateTime { get; set; }
		public DateTime? LastUpdateTime { get; set; }
		public int? MapFootballSeasonId { get; set; } // Dữ liệu Map - Nếu không map được thì upload ảnh lên rồi lưu ảnh trong
		public int? MapTeamId { get; set; }
		public string TeamLogoPath { get; set; }
		public int? MapLeagueId { get; set; }
		public string LeagueLogoPath { get; set; }
		public int? MapPlayerCareerId { get; set; }
		public bool IsProcessed { get; set; }
		public string BatchCode { get; set; }
		public string ProcessResult { get; set; }
		public DateTime? ProcessTime { get; set; }
	}
}
