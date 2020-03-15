namespace Fanex.Athena.ExtService.TagHelpers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [HtmlTargetElement("breadcrumb", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class BreadcrumbTagHelper : TagHelper
    {
        public IList<SelectListItem> Items { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "breadcrumb");

            foreach (var item in Items)
            {
                output.Content.AppendHtml($"<a class=\"{(item.Selected ? "active" : string.Empty)}\" href=\"{item.Value}\" >{item.Text}</a>");
            }
        }
    }
}