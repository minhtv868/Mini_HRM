using System.Text.Json.Serialization;

namespace Web.Application.Features.Finance.Categories.DTOs
{
    public class CategoryGetByParentIdDto : CategoryDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("text")]
        public string Name { get => GetNameByLevel(); }
    }
}
