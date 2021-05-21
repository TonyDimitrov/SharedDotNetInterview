namespace DotNetInterview.Web.ViewModels.Enums
{
    using DotNetInterview.Web.Infrastructure.CustomAttributes;

    public enum PositionSeniorityImgVM
    {
        [ViewDisplayAttribute(displayName: "other-p.png")]
        [ViewTooltipAttribute(tooltipClass: "Other")]
        None = 0,
        [ViewDisplayAttribute(displayName: "other-p.png")]
        [ViewTooltipAttribute(tooltipClass: "Other")]
        Other = 99,
        [ViewDisplayAttribute(displayName: "jun-dev.png")]
        [ViewTooltipAttribute(tooltipClass: "Junior dev")]
        JuniorDeveloper = 1,
        [ViewDisplayAttribute(displayName: "mid-dev.png")]
        [ViewTooltipAttribute(tooltipClass: "Regular dev")]
        RegularDeveloper = 2,
        [ViewDisplayAttribute(displayName: "sen-dev.png")]
        [ViewTooltipAttribute(tooltipClass: "Senior dev")]
        SeniorDeveloper = 3,
        [ViewDisplayAttribute(displayName: "tl.png")]
        [ViewTooltipAttribute(tooltipClass: "Team lead")]
        LeadDeveloper = 4,
        [ViewDisplayAttribute(displayName: "arch.png")]
        [ViewTooltipAttribute(tooltipClass: "Architect")]
        TechnicalArchitect = 5,
    }
}
