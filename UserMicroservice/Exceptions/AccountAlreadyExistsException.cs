using System;

namespace UserMicroservice.Exceptions
{
    [Serializable]
    public class AccountAlreadyExistsException : Exception
    {
        public AccountAlreadyExistsException()
            : base("A user with this account already exists.")
        {
        }
    }
}
