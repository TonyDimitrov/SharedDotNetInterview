namespace DotNetInterview.Services.Data.Extensions
{
    using Ganss.XSS;

    public static class StringExtensions
    {
        public static string FullUserNameParser(this string firstName, string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                if (firstName.Length <= 20)
                {
                    return firstName;
                }
                else
                {
                    return firstName.Substring(0, 17) + "...";
                }
            }
            else
            {
                var fullName = $"{firstName} {lastName.Substring(0, 1).ToUpper()}.";

                if (fullName.Length <= 20)
                {
                    return fullName;
                }
                else
                {
                    return fullName.Substring(0, 17) + "...";
                }
            }
        }

        public static string SanitizeTextInput(this string text)
        {
            var sanitizer = new HtmlSanitizer();

            return sanitizer.Sanitize(text);
        }
    }
}
