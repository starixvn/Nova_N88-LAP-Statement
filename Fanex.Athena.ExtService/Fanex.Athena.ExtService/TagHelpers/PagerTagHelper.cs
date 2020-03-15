namespace Fanex.Athena.ExtService.TagHelpers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Razor.TagHelpers;

    [HtmlTargetElement("pager", TagStructure = TagStructure.NormalOrSelfClosing)]
    public class PagerTagHelper : TagHelper
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalItems { get; set; }

        public string UrlTemplate { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";

            if (PageSize > 0 && PageIndex > 0)
            {
                var noOfPage = TotalItems / PageSize + (TotalItems % PageSize == 0 ? 0 : 1);
                output.Attributes.SetAttribute("class", "pagingContainer center-align");

                AddPageButton(output, "pagingFirst", PageIndex == 1, 1, PageSize);
                AddPageButton(output, "pagingPrev", PageIndex == 1, PageIndex - 1, PageSize);
                output.Content.AppendHtml("<span class=\"pagingSeperator\"></span>");
                output.Content.AppendHtml($"<span>Page {PageIndex} of {noOfPage}</span>");
                output.Content.AppendHtml("<span class=\"pagingSeperator\"></span>");
                AddPageButton(output, "pagingNext", PageIndex == noOfPage, PageIndex + 1, PageSize);
                AddPageButton(output, "pagingLast", PageIndex == noOfPage, noOfPage, PageSize);
                AddPageSizeOption(output);

                output.Content.AppendHtml($"<span class=\"paging-info\">View {(PageIndex - 1) * PageSize + 1}-{PageIndex * PageSize} of {TotalItems}</span>");
            }
        }

        private void AddPageButton(TagHelperOutput output, string className, bool isDisabled, int pageIndex, int pageSize)
        {
            output.Content.AppendHtml($"<a href=\"{BuildLink(pageIndex, pageSize)}\" class=\"icon {className} {(isDisabled ? "pagingDisable" : string.Empty)} \"></a>");
        }

        private void AddPageSizeOption(TagHelperOutput output)
        {
            var options = new List<int> { 50, 100, 200, 300, 400, 500, 1000, 2000 };

            output.Content.AppendHtml("<div class=\"pagingSelect\">");
            output.Content.AppendHtml($"<button>{PageSize}<i class=\"icon icon-xs-arrow\"></i></button>");
            output.Content.AppendHtml("<div class=\"pagingOption\">");         
            foreach (var option in options)
            {
                output.Content.AppendHtml($"<a href=\"{BuildLink(1, option)}\">{option}</a>");
            }
            output.Content.AppendHtml("</div>");
            output.Content.AppendHtml("</div>");
        }

        private string BuildLink(int pageIndex, int pageSize)
        {
            return UrlTemplate + $"&pi={pageIndex}&ps={pageSize}";
        }
    }
}
