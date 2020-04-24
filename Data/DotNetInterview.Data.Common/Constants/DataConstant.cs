namespace DotNetInterview.Data.Common.Constants
{
    public static class DataConstant
    {
        // User constants
        public const int NameMinLength = 2;
        public const int NameMaxLength = 20;
        public const int UserPositionMinLength = 2;
        public const int UserPositionMaxLength = 200;
        public const int UserDescriptionMinLength = 2;
        public const int UserDescriptionMaxLength = 1000;
        public const int EmailMinLength = 5;
        public const int EmailMaxLength = 100;

        // Interview constants
        public const int LocationlMinLength = 2;
        public const int LocationlMaxLength = 100;
        public const int CompanySizeMinLength = 1;
        public const int CompanySizeMaxLength = 1000_000;
        public const int PositionTitleMinLength = 2;
        public const int PositionTitleMaxLength = 200;
        public const int PositionDescriptionMinLength = 2;
        public const int PositionDescriptionMaxLength = 5000;
        public const int TagsMaxLength = 200;

        // Question constants
        public const int QuestionContentMinLength = 2;
        public const int QuestionContentMaxLength = 4000;
        public const int GivenAnswerMinLength = 2;
        public const int GivenAnswerMaxLength = 6000;
        public const int CorrectAnswerMinLength = 2;
        public const int CorrectAnswerMaxLength = 6000;

        // Comment constants
        public const int CommentContentMinLength = 2;
        public const int CommentContentMaxLength = 5000;

        // URL constants
        public const int UrlMinLength = 10;
        public const int UrlMaxLength = 300;
    }
}
