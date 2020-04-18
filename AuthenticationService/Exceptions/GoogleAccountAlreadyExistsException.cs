using System;

namespace AuthenticationService.Exceptions
{
    [Serializable]
    public class GoogleAccountAlreadyExistsException : Exception
    {
        public GoogleAccountAlreadyExistsException()
            : base("A user with this Google account already exists.")
        {
        }
    }
}
