using System;

namespace AuthenticationService.Exceptions
{
    [Serializable]
    public class IncorrectPasswordException : Exception
    {
        public IncorrectPasswordException()
            : base("The password is incorrect.")
        {
        }
    }
}
