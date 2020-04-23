namespace DotNetInterview.Web.Infrastructure.CustomValidationAttributes
{
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    public class ValidEmailAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            string email = value.ToString();

            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);

            if (match.Success)
            {
                return true;
            }

            this.ErrorMessage = $"Invalid email address!";

            return false;
        }
    }
}
