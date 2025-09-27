using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IC.WebCMS.Helpers.TagHelpers
{
    /// <summary>
    /// Tag Helper cho tính năng tự động hoàn thành thẻ (Tag Autocomplete)
    /// </summary>
    [HtmlTargetElement("tag-autocomplete", Attributes = ForAttributeName)]
    public class TagAutocompleteTagHelper : TagHelper
    {
        private const string ForAttributeName = "asp-for";
        private readonly IHtmlGenerator _htmlGenerator;

        /// <summary>
        /// Khởi tạo TagAutocompleteTagHelper với IHtmlGenerator
        /// </summary>
        /// <param name="htmlGenerator">Trình tạo HTML</param>
        public TagAutocompleteTagHelper(IHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator;
        }

        /// <summary>
        /// Biểu thức mô hình cho trường dữ liệu
        /// </summary>
        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        /// <summary>
        /// Ngữ cảnh chế độ xem
        /// </summary>
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Nhãn hiển thị cho trường nhập liệu
        /// </summary>
        [HtmlAttributeName("label")]
        public string Label { get; set; }

        /// <summary>
        /// Danh sách các thẻ gợi ý
        /// </summary>
        [HtmlAttributeName("suggestions")]
        public IEnumerable<string> Suggestions { get; set; }

        /// <summary>
        /// Giá trị đã chọn
        /// </summary>
        [HtmlAttributeName("selected-value")]
        public string SelectedValue { get; set; }

        /// <summary>
        /// URL từ xa để lấy dữ liệu gợi ý
        /// </summary>
        [HtmlAttributeName("remote-url")]
        public string RemoteUrl { get; set; }

        /// <summary>
        /// Chế độ tìm kiếm (local hoặc remote)
        /// </summary>
        [HtmlAttributeName("search-mode")]
        public string SearchMode { get; set; } = "local";

        /// <summary>
        /// Có yêu cầu nhập liệu hay không
        /// </summary>
        [HtmlAttributeName("is-required")]
        public bool IsRequired { get; set; } = false;

        /// <summary>
        /// Số ký tự tối thiểu để kích hoạt tìm kiếm
        /// </summary>
        [HtmlAttributeName("min")]
        public int Min { get; set; } = 1;

        /// <summary>
        /// Số lượng thẻ tối đa được phép chọn
        /// </summary>
        [HtmlAttributeName("max")]
        public int Max { get; set; } = 3;

        /// <summary>
        /// Hàm callback khi chọn thẻ
        /// </summary>
        [HtmlAttributeName("callback")]
        public string Callback { get; set; }

        /// <summary>
        /// Xử lý thẻ HTML và tạo giao diện tự động hoàn thành thẻ
        /// </summary>
        /// <param name="context">Ngữ cảnh của Tag Helper</param>
        /// <param name="output">Đầu ra của Tag Helper</param>
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "autocomplete-wrapper did-floating-input");
            output.Attributes.SetAttribute("data-for", For.Name);
            output.Attributes.SetAttribute("data-search-mode", SearchMode);
            output.Attributes.SetAttribute("data-min", Min.ToString());
            output.Attributes.SetAttribute("data-max", Max.ToString());
            if (!string.IsNullOrWhiteSpace(Callback))
            {
                output.Attributes.SetAttribute("data-callback", Callback);
            }

            if (!string.IsNullOrWhiteSpace(RemoteUrl))
            {
                output.Attributes.SetAttribute("data-url", RemoteUrl);
            }

            // Tạo container cho các thẻ và trường nhập liệu
            var tagContainer = new TagBuilder("div");
            tagContainer.AddCssClass("tag-container");

            // Kiểm tra nếu Model là danh sách đối tượng
            if (For.Model is IEnumerable<object> modelList)
            {
                int index = 0;
                foreach (var item in modelList)
                {
                    // Giả sử mỗi item có thuộc tính Id và Name
                    var idProp = item.GetType().GetProperty("Id");
                    var nameProp = item.GetType().GetProperty("Name");
                    if (idProp != null && nameProp != null)
                    {
                        var idValue = idProp.GetValue(item)?.ToString();
                        var nameValue = nameProp.GetValue(item)?.ToString();
                        if (!string.IsNullOrWhiteSpace(idValue) && !string.IsNullOrWhiteSpace(nameValue))
                        {
                            // Tạo badge cho thẻ
                            var badge = new TagBuilder("span");
                            badge.AddCssClass("tag-item badge bg-secondary");
                            badge.Attributes["data-id"] = idValue;
                            badge.Attributes["data-name"] = nameValue;
                            badge.InnerHtml.Append(nameValue);
                            var removeSpan = new TagBuilder("span");
                            removeSpan.AddCssClass("remove");
                            removeSpan.Attributes["title"] = "Bỏ chọn";
                            removeSpan.InnerHtml.Append("×");
                            badge.InnerHtml.AppendHtml(removeSpan);
                            tagContainer.InnerHtml.AppendHtml(badge);

                            // Tạo input hidden cho Id
                            var hiddenId = new TagBuilder("input");
                            hiddenId.Attributes["type"] = "hidden";
                            hiddenId.Attributes["name"] = $"{For.Name}[{index}].Id";
                            hiddenId.Attributes["value"] = idValue;
                            output.Content.AppendHtml(hiddenId);

                            // Tạo input hidden cho Name
                            var hiddenName = new TagBuilder("input");
                            hiddenName.Attributes["type"] = "hidden";
                            hiddenName.Attributes["name"] = $"{For.Name}[{index}].Name";
                            hiddenName.Attributes["value"] = nameValue;
                            output.Content.AppendHtml(hiddenName);

                            index++;
                        }
                    }
                }
            }

            // Tạo trường nhập liệu
            var input = new TagBuilder("input");
            input.AddCssClass("autocomplete-input");
            input.Attributes["type"] = "text";
            input.Attributes["placeholder"] = "Nhập từ khóa hoặc ấn Enter để chọn...";
            if (IsRequired)
            {
                input.Attributes["required"] = "required";
            }
            tagContainer.InnerHtml.AppendHtml(input);
            output.Content.AppendHtml(tagContainer);

            // Tạo danh sách gợi ý
            if (Suggestions != null && SearchMode == "local")
            {
                var ul = new TagBuilder("ul");
                ul.AddCssClass("tag-suggestions");
                foreach (var suggestion in Suggestions)
                {
                    var li = new TagBuilder("li");
                    li.Attributes["data-value"] = suggestion;
                    li.InnerHtml.Append(suggestion);
                    ul.InnerHtml.AppendHtml(li);
                }
                output.Content.AppendHtml(ul);
            }

            // Tạo label
            var displayLabel = !string.IsNullOrWhiteSpace(Label) ? Label : For.Metadata?.DisplayName ?? For.Metadata?.PropertyName ?? "Thẻ";
            var label = new TagBuilder("label");
            label.AddCssClass("did-floating-label floating");
            label.InnerHtml.Append(displayLabel);

            if (IsRequired)
            {
                var required = new TagBuilder("span");
                required.AddCssClass("text-danger");
                required.InnerHtml.Append("*");
                label.InnerHtml.AppendHtml(" ");
                label.InnerHtml.AppendHtml(required);
            }
            output.Content.AppendHtml(label);
        }
    }
}
