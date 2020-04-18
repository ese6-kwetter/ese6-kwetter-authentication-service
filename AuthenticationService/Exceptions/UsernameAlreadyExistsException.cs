using System;

namespace AuthenticationService.Exceptions
{
    [Serializable]
    public class UsernameAlreadyExistsException : Exception
    {
        public UsernameAlreadyExistsException()
            : base("A user with this username already exists.")
        {
        }
    }
}
