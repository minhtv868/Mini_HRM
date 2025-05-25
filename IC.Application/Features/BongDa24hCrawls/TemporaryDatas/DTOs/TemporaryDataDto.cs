using IC.Application.Common.Mappings;
using IC.Domain.Entities.BongDa24hCrawls;

namespace IC.Application.Features.BongDa24hCrawls.TemporaryDatas.DTOs
{
    public class TemporaryDataDto : IMapFrom<TemporaryData>
    {
        public int Id { get; set; }
        public string DataSouceName { get; set; }
        public string DataAction { get; set; }
        public string DataId { get; set; }
        public string DataJson { get; set; }
        public long? Hash { get; set; }
        public string BatchCode { get; set; }
        public DateTime? CrDateTime { get; set; }
    }
}
