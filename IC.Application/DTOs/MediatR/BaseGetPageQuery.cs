using System.ComponentModel;

namespace IC.Application.DTOs.MediatR
{
    public record BaseGetPageQuery
    {
        [DisplayName("Từ khóa")]
        public string Keywords { get; set; }
        public int Page { get; set; }
        [DisplayName("Số lượng")]
        public int PageSize { get; set; }
    }
}
