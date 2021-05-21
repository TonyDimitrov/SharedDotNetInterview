namespace DotNetInterview.Web.Infrastructure.CustomAttributes
{
    using System;

    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public class ViewTooltipAttribute : Attribute
    {
        public ViewTooltipAttribute(string tooltipClass)
        {
            this.TooltipClass = tooltipClass;
        }

        public string TooltipClass { get; private set; }
    }
}