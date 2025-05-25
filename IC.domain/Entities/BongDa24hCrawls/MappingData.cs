using IC.Domain.Common;
using System;

namespace IC.Domain.Entities.BongDa24hCrawls
{
    public class MappingData : BaseAuditableEntity
    {
        public int Id { get; set; }
        public string DataSouceName { get; set; }
        public int DataId { get; set; }
        public string DataKey { get; set; }
        public DateTime? CrDateTime { get; set; }
    }
}