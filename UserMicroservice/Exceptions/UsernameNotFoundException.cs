using System;

namespace UserMicroservice.Exceptions
{
    [Serializable]
    public class UsernameNotFoundException : Exception
    {
        public UsernameNotFoundException()
            : base("A user with this username was not found.")
        {
        }
    }
}
