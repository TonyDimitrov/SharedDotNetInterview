namespace DotNetInterview.Web.ViewModels.Common
{
    public class DbOperation
    {
        public DbOperation(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
        }

        public bool Success { get; }

        public string Message { get; }
    }
}
