using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Text.Encodings.Web;
using System.Text.Json;
using Web.Shared.Helpers;

namespace IC.WebCMS.Helpers.TagHelpers.SmartSelect
{
    public abstract class SmartSelectBaseTagHelper : TagHelper
    {
        protected const string ForAttributeName = "asp-for";
        private readonly IHtmlGenerator _htmlGenerator;

        protected SmartSelectBaseTagHelper(IHtmlGenerator htmlGenerator)
        {
            _htmlGenerator = htmlGenerator;
        }

        [HtmlAttributeName(ForAttributeName)]
        public ModelExpression For { get; set; }

        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeName("label")]
        public string Label { get; set; }

        [HtmlAttributeName("enum-type")]
        public Type EnumType { get; set; }

        [HtmlAttributeName("use-enum-name")]
        public bool UseEnumName { get; set; } = false;

        [HtmlAttributeName("items")]
        public IEnumerable<SelectListItem> Items { get; set; }

        [HtmlAttributeName("selected-text")]
        public string SelectedText { get; set; }
        [HtmlAttributeName("target")]
        public string Target { get; set; }

        [HtmlAttributeName("selected-value")]
        public string SelectedValue { get; set; }

        [HtmlAttributeName("default-display-text")]
        public string DefaultDisplayText { get; set; }

        [HtmlAttributeName("submit-on-change")]
        public bool SubmitOnChange { get; set; } = true;

        [HtmlAttributeName("remote-url")]
        public string RemoteUrl { get; set; }

        [HtmlAttributeName("update-url")]
        public string UpdateUrl { get; set; }
        [HtmlAttributeName("update-id")]
        public string IdUpdate { get; set; }
        [HtmlAttributeName("update-params")]
        public string Params { get; set; }

        [HtmlAttributeName("search-mode")]
        public string SearchMode { get; set; } = "local";

        [HtmlAttributeName("callback-function")]
        public string CallbackFunction { get; set; }

        [HtmlAttributeName("callback-args")]
        public object CallbackArgs { get; set; }

        [HtmlAttributeName("default-value")]
        public string DefaultValue { get; set; } = "0";

        [HtmlAttributeName("show-default-option")]
        public bool ShowDefaultOption { get; set; } = true;

        [HtmlAttributeName("is-required")]
        public bool IsRequired { get; set; } = false;

        [HtmlAttributeName("use-native-select")]
        public bool UseNativeSelect { get; set; } = true;
        [HtmlAttributeName("multiple")]
        public bool Multiple { get; set; } = false;

        protected void GenerateSmartSelect(TagHelperOutput output, bool includeValidation, bool useHiddenInput)
        {
            if (Items == null && EnumType?.IsEnum == true)
            {
                Items = Enum.GetValues(EnumType)
                    .Cast<object>()
                    .Select(e => new SelectListItem
                    {
                        Value = UseEnumName
                            ? e.ToString()
                            : Convert.ChangeType(e, Enum.GetUnderlyingType(EnumType)).ToString(),
                        Text = ((Enum)e).GetDisplayName()
                    })
                    .ToList();
            }

            var fieldName = For.Name;
            var selectedValue = !string.IsNullOrWhiteSpace(SelectedValue)
                ? SelectedValue
                : For.Model?.ToString() ?? "";

            var selectedItemText = !string.IsNullOrWhiteSpace(SelectedText)
                ? SelectedText
                : Items?.FirstOrDefault(x => x.Value == selectedValue)?.Text;

            selectedItemText ??= "...";

            var defaultDisplayText = DefaultDisplayText;
            //!string.IsNullOrWhiteSpace(DefaultDisplayText)
            //? DefaultDisplayText
            //: !string.IsNullOrWhiteSpace(selectedItemText) ? selectedItemText : "...";

            var callbackArgsJson = JsonSerializer.Serialize(CallbackArgs ?? new object(), new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            output.TagName = "div";
            output.TagMode = TagMode.StartTagAndEndTag;
            output.Attributes.SetAttribute("class", "smart-select did-floating-select");
            output.Attributes.SetAttribute("data-for", fieldName);
            output.Attributes.SetAttribute("data-submit", SubmitOnChange.ToString().ToLower());
            output.Attributes.SetAttribute("data-search-mode", SearchMode);
            output.Attributes.SetAttribute("data-show-default-option", ShowDefaultOption.ToString().ToLower());

            if (!string.IsNullOrWhiteSpace(CallbackFunction))
                output.Attributes.SetAttribute("data-callback-function", CallbackFunction);
            if (!string.IsNullOrWhiteSpace(callbackArgsJson))
                output.Attributes.SetAttribute("data-callback-args", callbackArgsJson);
            if (!string.IsNullOrWhiteSpace(RemoteUrl))
                output.Attributes.SetAttribute("data-url", RemoteUrl);
            if (!string.IsNullOrWhiteSpace(Target))
                output.Attributes.SetAttribute("data-target", Target);
            if (!string.IsNullOrWhiteSpace(UpdateUrl))
                output.Attributes.SetAttribute("data-update-url", UpdateUrl);
            if (!string.IsNullOrWhiteSpace(IdUpdate))
                output.Attributes.SetAttribute("data-update-id", IdUpdate);
            if (!string.IsNullOrWhiteSpace(Params))
                output.Attributes.SetAttribute("data-update-param", Params);
            // dropdown UI
            var dropdown = new TagBuilder("div");
            dropdown.AddCssClass("select-dropdown select-active");
            var spanText = new TagBuilder("span");
            spanText.Attributes.Add("data-initial-text", selectedItemText.StripTags());
            spanText.InnerHtml.AppendHtml(new HtmlString(selectedItemText));
            dropdown.InnerHtml.AppendHtml(spanText);

            var spanIcon = new TagBuilder("span");
            spanIcon.AddCssClass("icon-select");
            var spinner = new TagBuilder("span");
            spinner.AddCssClass("loading-spinner");
            spanIcon.InnerHtml.AppendHtml(spinner);
            dropdown.InnerHtml.AppendHtml(spanIcon);

            output.Content.AppendHtml(dropdown);

            // search + options
            var wrapper = new TagBuilder("div");
            wrapper.AddCssClass("select-wrapper");

            if (!string.Equals(SearchMode, "none", StringComparison.OrdinalIgnoreCase))
            {
                var searchDiv = new TagBuilder("div");
                searchDiv.AddCssClass("select-search");

                var input = new TagBuilder("input");
                input.AddCssClass("input full search-input");
                input.Attributes.Add("placeholder", "Tìm kiếm...");
                searchDiv.InnerHtml.AppendHtml(input);

                var searchGroup = new TagBuilder("span");
                searchGroup.AddCssClass("search-group");
                var icon = new TagBuilder("i");
                icon.AddCssClass("fa fa-times clear-search");
                icon.Attributes.Add("title", "Xóa từ khóa");
                icon.Attributes.Add("style", "cursor: pointer; display: none;");
                searchGroup.InnerHtml.AppendHtml(icon);

                searchDiv.InnerHtml.AppendHtml(searchGroup);
                wrapper.InnerHtml.AppendHtml(searchDiv);
            }

            var contentDiv = new TagBuilder("div");
            contentDiv.AddCssClass("select-content");
            var ul = new TagBuilder("ul");
            ul.AddCssClass("ul-option");

            if (ShowDefaultOption)
            {
                var defaultLi = new TagBuilder("li");
                defaultLi.Attributes["data-value"] = DefaultValue;
                if (selectedValue == DefaultValue) defaultLi.AddCssClass("selected");
                defaultLi.InnerHtml.Append(defaultDisplayText);
                ul.InnerHtml.AppendHtml(defaultLi);
            }

            foreach (var item in Items ?? Enumerable.Empty<SelectListItem>())
            {
                var li = new TagBuilder("li");
                li.Attributes["data-value"] = item.Value;
                if (item.Value == selectedValue)
                    li.AddCssClass("selected");
                //li.InnerHtml.Append(item.Text);
                li.InnerHtml.AppendHtml(new HtmlString(item.Text));
                ul.InnerHtml.AppendHtml(li);
            }

            contentDiv.InnerHtml.AppendHtml(ul);
            wrapper.InnerHtml.AppendHtml(contentDiv);
            output.Content.AppendHtml(wrapper);

            // select hoặc input hidden
            if (UseNativeSelect && !useHiddenInput)
            {
                bool hasNoItems = Items == null || !Items.Any();
                bool isSmartSource = EnumType != null || !string.IsNullOrEmpty(RemoteUrl);

                if (hasNoItems && isSmartSource)
                {
                    var warning = new TagBuilder("div");
                    warning.Attributes["style"] = "color: red; font-weight: bold;";
                    warning.InnerHtml.Append("[SmartSelect] ⚠️ Không thể dùng native select với enum-type hoặc remote-url. Vui lòng thêm ");
                    var code = new TagBuilder("code");
                    code.InnerHtml.Append("use-native-select=\"false\"");
                    warning.InnerHtml.AppendHtml(code);
                    warning.InnerHtml.Append(".");
                    output.Content.AppendHtml(warning);
                    return;
                }

                var selectAttributes = new Dictionary<string, object> { { "class", "d-none" } };

                var selectTag = _htmlGenerator.GenerateSelect(
                    ViewContext,
                    For.ModelExplorer,
                    expression: For.Name,
                    selectList: Items,
                    optionLabel: ShowDefaultOption ? "..." : null,
                    allowMultiple: false,
                    htmlAttributes: new Dictionary<string, object> { { "class", "d-none" } }
                );


                output.Content.AppendHtml(selectTag);

                if (includeValidation)
                {
                    var validationTag = _htmlGenerator.GenerateValidationMessage(ViewContext, For.ModelExplorer, For.Name, null, null, new { @class = "text-danger" });
                    output.Content.AppendHtml(validationTag);
                }
            }
            else if (useHiddenInput)
            {
                var hiddenInput = new TagBuilder("input");
                hiddenInput.AddCssClass("select-hidden");
                hiddenInput.Attributes["type"] = "hidden";
                hiddenInput.Attributes["name"] = fieldName;
                hiddenInput.Attributes["value"] = selectedValue;
                output.Content.AppendHtml(hiddenInput);
            }

            // label
            if (string.IsNullOrWhiteSpace(Label) && For?.Metadata != null)
            {
                Label = For.Metadata.DisplayName ?? For.Metadata.PropertyName;
            }

            var labelTag = new TagBuilder("label");
            labelTag.AddCssClass("did-floating-label floating");
            labelTag.InnerHtml.Append(Label);
            if (IsRequired)
            {
                var required = new TagBuilder("span");
                required.AddCssClass("text-danger");
                required.InnerHtml.Append("*");
                labelTag.InnerHtml.AppendHtml(" ");
                labelTag.InnerHtml.AppendHtml(required);
            }

            output.Content.AppendHtml(labelTag);
        }
    }
}
