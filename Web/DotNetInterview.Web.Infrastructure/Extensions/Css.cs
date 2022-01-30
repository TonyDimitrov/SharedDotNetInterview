namespace DotNetInterview.Web.Infrastructure.Extensions
{
    public static class Css
    {
        public static string ActiveLink(this bool isActive, string element)
        {
            var cssClass = string.Format("{0}-active", element).ToLower();

            if (isActive)
            {
                return cssClass;
            }

            return string.Empty;
        }
    }
}
