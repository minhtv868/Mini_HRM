using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace IC.WebCMS.Helpers.TagHelpers.SmartSelect
{
    [HtmlTargetElement("smart-select-filter", Attributes = ForAttributeName)]
    public class SmartSelectFilterTagHelper : SmartSelectBaseTagHelper
    {
        public SmartSelectFilterTagHelper(IHtmlGenerator htmlGenerator) : base(htmlGenerator)
        {
            UseNativeSelect = false; // override mặc định
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            base.GenerateSmartSelect(output, includeValidation: false, useHiddenInput: true);
        }
    }
}
