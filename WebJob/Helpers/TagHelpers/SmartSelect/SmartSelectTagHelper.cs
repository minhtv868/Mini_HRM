using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IC.WebCMS.Helpers.TagHelpers.SmartSelect
{
    [HtmlTargetElement("smart-select", Attributes = ForAttributeName)]
    public class SmartSelectTagHelper : SmartSelectBaseTagHelper
    {
        public SmartSelectTagHelper(IHtmlGenerator htmlGenerator) : base(htmlGenerator)
        {
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.GenerateSmartSelect(output, includeValidation: true, useHiddenInput: false);
        }
    }
}
