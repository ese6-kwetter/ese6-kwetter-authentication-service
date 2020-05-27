using System;

namespace UserMicroservice.Exceptions
{
    [Serializable]
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException()
            : base("The password is not valid. " +
                   "Make sure the password is minimal 8 characters long and contains at least: " +
                   "1 lower case, 1 upper case, 1 number and 1 special character.")
        {
        }
    }
}
