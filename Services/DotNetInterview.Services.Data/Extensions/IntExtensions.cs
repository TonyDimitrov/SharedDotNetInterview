namespace DotNetInterview.Services.Data.Extensions
{
    using System;

    public static class IntExtensions
    {
     public static string SeniorityNameParser(this int seniority) =>
            seniority switch
            {
                0 => "Not specified",
                1 => "Junior developer",
                2 => "Regular developer",
                3 => "Senior developer",
                4 => "Lead developer",
                5 => "Architect",
                99 => "Other",
                _ => throw new ArgumentException($"Seniority type [{seniority}] is invalid!"),
            };
    }
}
