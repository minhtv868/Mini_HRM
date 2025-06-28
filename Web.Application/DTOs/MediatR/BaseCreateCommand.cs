using System.ComponentModel;

namespace Web.Application.DTOs.MediatR
{
    public record BaseCreateCommand
    {
        [DisplayName("Thêm tiếp dữ liệu khác")]
        public bool AddMoreData { get; set; }
    }
}
