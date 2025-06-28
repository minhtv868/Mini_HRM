using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebJob.Helpers
{
    [HtmlTargetElement("select", Attributes = PlaceholderAttributeName)]
    [HtmlTargetElement("textarea", Attributes = PlaceholderAttributeName)]
    [HtmlTargetElement("input", Attributes = PlaceholderAttributeName, TagStructure = TagStructure.WithoutEndTag)]
    public class InputPlaceholderTagHelper : InputTagHelper
    {
        private const string PlaceholderAttributeName = "asp-placeholder-for";

        public InputPlaceholderTagHelper(IHtmlGenerator generator) : base(generator)
        {
        }

        [HtmlAttributeName(PlaceholderAttributeName)]
        public ModelExpression Placeholder { get; set; }


        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.Process(context, output);

            var placeholder = GetPlaceholder(Placeholder.ModelExplorer);
            TagHelperAttribute placeholderAttribute;

            if (!output.Attributes.TryGetAttribute("placeholder", out placeholderAttribute))
            {
                output.Attributes.Add(new TagHelperAttribute("placeholder", placeholder));
            }
        }

        private string GetPlaceholder(ModelExplorer modelExplorer)
        {
            string placeholder;
            placeholder = modelExplorer.Metadata.Placeholder;

            if (string.IsNullOrWhiteSpace(placeholder))
            {
                placeholder = modelExplorer.Metadata.GetDisplayName();
            }

            return placeholder;
        }
    }
    
    [HtmlTargetElement("textarea", Attributes = "asp-readonly")]
    [HtmlTargetElement("input", Attributes = "asp-readonly", TagStructure = TagStructure.WithoutEndTag)]
    public class ReadOnlyTagHelper : TagHelper
    {
        public bool AspReadonly { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (AspReadonly)
            {   
                output.Attributes.SetAttribute("readonly", "readonly");
            }
        }
    }
    [HtmlTargetElement("select", Attributes = "asp-readonly")]
    public class ReadOnlySelectTagHelper : TagHelper
    {
        public bool AspReadonly { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (AspReadonly)
            {
                output.Attributes.SetAttribute("disabled", "disabled");
            }
        }
    }
    [HtmlTargetElement("label-required")]
    public class FloatingLabelRequiredTagHelper : TagHelper
    {
        public ModelExpression AspFor { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "label";
            output.Content.Append(this.AspFor.Metadata.GetDisplayName());
            output.Content.AppendHtml(@"<span class=""text-danger"">*</span>");
        }
    }
}
