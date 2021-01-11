namespace DotNetInterview.Web.ViewModels.Enums
{
    using DotNetInterview.Web.Infrastructure.CustomAttributes;

    public enum QuestionRankImgType
    {
        [ViewDisplay("")]
        None = 0,
        [ViewDisplay("most-interesting.png")]
        MostInteresting = 1,
        [ViewDisplay("most-unexpected.png")]
        MostUnexpected = 2,
        [ViewDisplay("most-difficult.png")]
        MostDifficult = 3,
        Other = 999,
    }
}
