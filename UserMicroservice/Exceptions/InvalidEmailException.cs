using System;

namespace UserMicroservice.Exceptions
{
    [Serializable]
    public class InvalidEmailException : Exception
    {
        public InvalidEmailException()
            : base("The email is invalid. " +
                   "Make sure it has an '@' and '.' in the email address.")
        {
        }
    }
}
