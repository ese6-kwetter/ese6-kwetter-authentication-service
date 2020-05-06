using System;

namespace UserMicroservice.Exceptions
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
