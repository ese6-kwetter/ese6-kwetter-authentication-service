using System;

namespace UserMicroservice.Exceptions
{
    [Serializable]
    public class AccountNotFoundException : Exception
    {
        public AccountNotFoundException()
            : base("A user with this account was not found.")
        {
        }
    }
}
