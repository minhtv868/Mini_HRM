using System.Text.Json.Serialization;

namespace Web.Application.Features.Finance.Categories.DTOs
{
    public class CategorySearchDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("text")]
        public string Name { get; set; }

        public CategorySearchDto() { }

        public CategorySearchDto(int id, string name, byte? treeLevel)
        {
            Id = id;
            Name = GetNameByLevel(name, treeLevel, true);
        }

        public string GetNameByLevel(string name, byte? treeLevel, bool forSelectItems = false)
        {
            var itemName = name;

            var textPrefix = forSelectItems ? "-" : "&nbsp;&nbsp;";

            if (!treeLevel.HasValue || treeLevel <= 1)
            {
                if (!forSelectItems)
                {
                    itemName = $"<b>{itemName}</b>";
                }
                ;
            }
            else
            {
                textPrefix = string.Join("", Enumerable.Repeat(textPrefix, treeLevel.Value).ToArray());

                itemName = $"{textPrefix}{itemName}";
            }

            return itemName;
        }
    }
}
