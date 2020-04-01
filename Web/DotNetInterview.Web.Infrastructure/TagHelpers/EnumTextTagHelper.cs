namespace DotNetInterview.Web.Infrastructure.TagHelpers
{
    using System.Text.RegularExpressions;

    using Microsoft.AspNetCore.Razor.TagHelpers;

    [HtmlTargetElement(Attributes = "enum-text")]
    public class EnumTextTagHelper : TagHelper
    {
        private const string Separator = " ";

        public string EnumText { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var r = new Regex(
                @"
                 (?<=[A-Z])(?=[A-Z][a-z]) |
                 (?<=[^A-Z])(?=[A-Z]) |
                 (?<=[A-Za-z])(?=[^A-Za-z])", RegexOptions.IgnorePatternWhitespace);

            output.Content.SetContent(r.Replace(this.EnumText, Separator));
            base.Process(context, output);
        }
    }
}