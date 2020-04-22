using System;

namespace UserService.Exceptions
{
    [Serializable]
    public class AppleAccountAlreadyExistsException : Exception
    {
        public AppleAccountAlreadyExistsException()
            : base("A user with this Apple account already exists.")
        {
        }
    }
}
