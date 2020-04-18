using System;

namespace AuthenticationService.Exceptions
{
    [Serializable]
    public class EmailNotFoundException : Exception
    {
        public EmailNotFoundException()
            : base("A user with this email was not found.")
        {
        }
    }
}
