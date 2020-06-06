using System.Collections.Generic;
using System.Linq;
using UserMicroservice.Entities;

namespace UserMicroservice.Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<User> WithoutSensitiveData(this IEnumerable<User> users)
        {
            return users.Select(x => x.WithoutSensitiveData());
        }

        public static User WithoutSensitiveData(this User user)
        {
            user.Password = null;
            user.Salt = null;

            return user;
        }
    }
}
