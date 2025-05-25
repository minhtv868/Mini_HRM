using IC.Domain.Common;
using System;

namespace IC.Domain.Entities.BongDa24hCrawls
{
	public class PlayerInjuryHistoryCrawl : BaseAuditableEntity
	{
		public int Id { get; set; }
		public string UrlCrawl { get; set; }
		public int PlayerId { get; set; }
		public DateTime FromTime { get; set; } 
		public DateTime? UntilTime { get; set; }
		public string InjuryType { get; set; }
		public int Sort { get; set; } 
		public long? ContentHash { get; set; } // Bao gồm PlayerId, FromTime, UntilTime, InjuryType, Sort
		public DateTime CrDateTime { get; set; } 
		public DateTime? LastUpdateTime { get; set; }
		public int? MapPlayerInjuryHistoryId { get; set; }
		public bool IsProcessed { get; set; } = false;
		public string BatchCode { get; set; }
		public string ProcessResult { get; set; }
		public DateTime? ProcessTime { get; set; }
	}
}