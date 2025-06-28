using Web.Application.Common.Mappings;
using Web.Domain.Entities.Crawls;

namespace Web.Application.Features.BongDa24hCrawls.MappingDatas.DTOs
{
    public class MappingDataDto : IMapFrom<MappingData>
    {
        public int Id { get; set; }
        public string DataSouceName { get; set; }
        public int DataId { get; set; }
        public string DataKey { get; set; }
        public DateTime? CrDateTime { get; set; }
    }
}
