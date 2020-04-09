namespace DotNetInterview.Web.Infrastructure.CustomValidationAttributes
{
    using System.Collections;
    using System.ComponentModel.DataAnnotations;

    public class CollectionMinLengthAttribute : ValidationAttribute
    {
        private readonly int minLength;

        public CollectionMinLengthAttribute(int minLength)
        {
            this.minLength = minLength;
        }

        public override bool IsValid(object value)
        {
            var collection = value as ICollection;

            if (collection == null || collection.Count < this.minLength)
            {
                this.ErrorMessage = $"Collection should have minimum {this.minLength} element!";
                return false;
            }

            return true;
        }
    }
}
