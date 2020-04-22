using System;

namespace UserService.Exceptions
{
    [Serializable]
    public class EmailAlreadyExistsException : Exception
    {
        public EmailAlreadyExistsException()
            : base("A user with this email already exists.")
        {
        }
    }
}
