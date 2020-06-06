using System.Threading.Tasks;
using UserMicroservice.Entities;

namespace UserMicroservice.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        /// <summary>
        ///     Gets a single user by their username.
        /// </summary>
        /// <param name="username">The username to search for</param>
        /// <returns>User with the correct username</returns>
        Task<User> ReadByUsernameAsync(string username);

        /// <summary>
        ///     Gets a single user by their email.
        /// </summary>
        /// <param name="email">The email to search for</param>
        /// <returns>User with the correct email</returns>
        Task<User> ReadByEmailAsync(string email);
    }
}
