using System;

namespace UserMicroservice.Exceptions
{
    [Serializable]
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException()
            : base("The password is invalid.")
        {
        }
    }
}
