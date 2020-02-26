using DotNetInterview.Web.ViewModels.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetInterview.Web.ViewModels.Interviews
{
    public class CreateInterviewQuestionVM
    {
        public string Content { get; set; }

        public byte[] FileTask { get; set; }

        public string UrlTask { get; set; }

        public string UrlGitRepo { get; set; }

        public string GivenAnswer { get; set; }

        public string CorrectAnswer { get; set; }

        public int Interesting { get; set; }

        public int Unexpected { get; set; }

        public int Difficult { get; set; }
    }
}
