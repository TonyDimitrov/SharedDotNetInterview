namespace DotNetInterview.Web.Infrastructure.TagHelpers
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    using DotNetInterview.Common;
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

            var capitalizationFixed = this.CapitalizeOnlyFirstLetterOfFirstWord(r.Replace(this.EnumText, Separator));
            output.Content.SetContent(capitalizationFixed);
            base.Process(context, output);
        }

        private string CapitalizeOnlyFirstLetterOfFirstWord(string replacedText)
        {
            if (string.IsNullOrEmpty(replacedText))
            {
                return null;
            }

            var words = replacedText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length == 1)
            {
                return replacedText == GlobalConstants.None ? GlobalConstants.NoInformation : replacedText;
            }

            StringBuilder sb = new StringBuilder(words[0]);

            for (int i = 1; i < words.Length; i++)
            {
                sb.Append(" ");
                sb.Append(string.Format(words[i].ToLower()));
            }

            return sb.ToString();
        }
    }
}