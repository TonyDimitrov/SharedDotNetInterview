namespace DotNetInterview.Web.ViewModels.Enums
{
    using DotNetInterview.Web.Infrastructure.CustomAttributes;

    public enum PositionSeniorityImgVM
    {
        [ViewDisplayAttribute(displayName: "other.png")]
        None = 0,
        [ViewDisplayAttribute(displayName: "other.png")]
        Other = 99,
        [ViewDisplayAttribute(displayName: "j.png")]
        JuniorDeveloper = 1,
        [ViewDisplayAttribute(displayName: "r.png")]
        RegularDeveloper = 2,
        [ViewDisplayAttribute(displayName: "s.png")]
        SeniorDeveloper = 3,
        [ViewDisplayAttribute(displayName: "t.png")]
        LeadDeveloper = 4,
        [ViewDisplayAttribute(displayName: "a.png")]
        TechnicalArchitect = 5,
    }
}
