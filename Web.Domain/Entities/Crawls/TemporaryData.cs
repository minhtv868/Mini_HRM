using Web.Domain.Common;
namespace Web.Domain.Entities.Crawls
{
    public class TemporaryData : BaseAuditableEntity
    {
        public int Id { get; set; }
        public string DataSouceName { get; set; }
        public string DataId { get; set; }
        public string DataAction { get; set; }
        public string DataJson { get; set; }
        public long? Hash { get; set; }
        public string BatchCode { get; set; }
        public DateTime? CrDateTime { get; set; }
    }
}
