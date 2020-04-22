using System;

namespace UserService.Exceptions
{
    [Serializable]
    public class GoogleAccountNotFoundException : Exception
    {
        public GoogleAccountNotFoundException()
            : base("A user with this Google account was not found.")
        {
        }
    }
}
