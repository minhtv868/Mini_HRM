using Web.Domain.Common;
using System;

namespace Web.Domain.Entities.Crawls
{
	public class PlayerTransferHistoryCrawl : BaseAuditableEntity
	{
		public int Id { get; set; }
		public string UrlCrawl { get; set; }
		public int PlayerId { get; set; }
		public DateTime TransferDate { get; set; }
		public long? KeyHash { get; set; } // Bao gồm PlayerId, TransferDate, Type
		public string FromTeamName { get; set; }
		public string FromTeamLogo { get; set; }
		public string FromTeamUrl { get; set; }
		public string ToTeamName { get; set; }
		public string ToTeamLogo { get; set; }
		public string ToTeamUrl { get; set; }
		public string Type { get; set; }
		public string Fee { get; set; }
		public float? TransferFee { get; set; }
		public int Sort { get; set; } = 0;
		public long? ContentHash { get; set; } // Bao gồm PlayerId, FromTeamName, ToTeamName, Type, Fee, Sort
		public DateTime CrDateTime { get; set; } 
		public DateTime? LastUpdateTime { get; set; }
		public int? MapFromTeamId { get; set; }
		public string FromTeamLogoPath { get; set; }
		public int? MapToTeamId { get; set; }
		public string ToTeamLogoPath { get; set; }
		public int? MapPlayerTransferHistoryId { get; set; }
		public bool IsProcessed { get; set; } = false;
		public string BatchCode { get; set; }
		public string ProcessResult { get; set; }
		public DateTime? ProcessTime { get; set; }
	}
}
