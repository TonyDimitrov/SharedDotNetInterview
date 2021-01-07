namespace DotNetInterview.Web.Infrastructure.CustomAttributes
{
    using System;

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ViewDisplayAttribute : Attribute
    {
        public ViewDisplayAttribute(string displayName)
        {
            this.DisplayName = displayName;
        }

        public string DisplayName { get; set; }
    }
}
