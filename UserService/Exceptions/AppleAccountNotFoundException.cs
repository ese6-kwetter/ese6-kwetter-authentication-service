using System;

namespace UserService.Exceptions
{
    [Serializable]
    public class AppleAccountNotFoundException : Exception
    {
        public AppleAccountNotFoundException()
            : base("A user with this Apple account was not found.")
        {
        }
    }
}
