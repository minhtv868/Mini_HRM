using Web.Application.Common.Mappings;
using Web.Domain.Entities.Finance;

namespace Web.Application.Features.Finance.UrlCrawls.DTOs
{
    public class UrlCrawlGetAllDto : IMapFrom<UrlCrawl>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public byte DataType { get; set; }
        public int? DataId { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CrDateTime { get; set; }

    }
}
