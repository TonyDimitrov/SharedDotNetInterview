namespace DotNetInterview.Web.ViewModels
{
    public class ErrorVM
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(this.RequestId);

        public string Message { get; set; }
    }
}
