using System;

namespace UserMicroservice.Exceptions
{
    [Serializable]
    public class InvalidEmailException : Exception
    {
        public InvalidEmailException()
            : base("The email is invalid.")
        {
        }
    }
}
