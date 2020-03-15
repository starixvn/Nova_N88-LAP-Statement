using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Fanex.Athena.ExtService.TagHelpers
{
    [HtmlTargetElement("money", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class MoneyTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "span";
            var content = await output.GetChildContentAsync().ConfigureAwait(false);
            var isNumeric = Decimal.TryParse(content.GetContent(), out decimal number);

            if (isNumeric)
            {
                output.Attributes.SetAttribute("class", number >= 0 ? "underdog" : "favorite");
                output.Content.SetContent($"{number:#,##0.00}");
            }
        }
    }
}