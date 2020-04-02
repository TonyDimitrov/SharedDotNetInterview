using System;

namespace DotNetInterview.Services.Data.CustomExceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string error)
        {
            this.CustomMessage = error;
        }

        public string CustomMessage { get; private set; }
    }
}
