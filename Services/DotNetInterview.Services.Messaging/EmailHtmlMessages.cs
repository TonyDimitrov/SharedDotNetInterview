namespace DotNetInterview.Services.Messaging
{
    public static class EmailHtmlMessages
    {
        public static string ResetPasswordHtml(string ressetPasswordLink, string username = null)
        {
            return $"<div style=\"color: #0c3e72; font-family: Open Sans, sans-serif;\">" +
                    $"  <h1>FORGOT</h1>" +
                    $"  <h3> YOU PASSWORD!</h3>" +
                    $"  <br>" +
                    $"  <p>Not to worry, we got you! Let’s get you a new password.</p>" +
                    $"  <br>" +
                    $"  <a href=\"{ressetPasswordLink}\" style=\"color: #fe5c24;\">Reset password</a>" +
                    $"</div>";
        }

        public static string ConfirmEmailHtml(string ressetPasswordLink, string username = null)
        {
            return $" <div style=\"color: #0c3e72; font-family: Open Sans, sans-serif;\">" +
                     $"   <h1>Confirm your email address</h1>" +
                     $"   <h3>It is easy</h3>" +
                     $"   <br>" +
                     $"   <p>Just click on the link below.</p>" +
                     $"   <br>" +
                     $"   <a href=\"{ressetPasswordLink}\" style=\"color: #fe5c24;\">Confirm</a>" +
                     $" </div>";
        }
    }
}
