using System.Text.Json.Serialization;

namespace IC.Application.DTOs.DictData
{
    public class SelectListItemDto
    {
		[JsonPropertyName("id")]
		public int Id { get; set; }
		[JsonPropertyName("text")]
		public string Name { get; set; }
		[JsonPropertyName("siteid")]
		public short siteid {  get; set; }
    }
}
