using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UserService.Entities;

namespace UserService.Repositories
{
    public interface IUserRepository
    {
        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="user">User to be saved</param>
        /// <returns>Created user</returns>
        Task<User> Create(User user);
        
        /// <summary>
        /// Gets a list of all the users
        /// </summary>
        /// <returns>List of all the users</returns>
        Task<List<User>> Read();

        /// <summary>
        /// Gets a single user by their Guid
        /// </summary>
        /// <param name="id">The guid to search for</param>
        /// <returns>User with the correct guid</returns>
        Task<User> Read(Guid id);

        /// <summary>
        /// Gets a single user by their username
        /// </summary>
        /// <param name="username">The username to search for</param>
        /// <returns>User with the correct username</returns>
        Task<User> ReadByUsername(string username);

        /// <summary>
        /// Gets a single user by their email
        /// </summary>
        /// <param name="email">The email to search for</param>
        /// <returns>User with the correct email</returns>
        Task<User> ReadByEmail(string email);

        /// <summary>
        /// Updates an existing user
        /// </summary>
        /// <param name="id">Guid of the user</param>
        /// <param name="userIn">User with updated fields</param>
        /// <returns>Updated user</returns>
        Task Update(Guid id, User userIn);

        /// <summary>
        /// Removes an user by their Guid
        /// </summary>
        /// <param name="id">Guid of the user to remove</param>
        /// <returns>Async task to await</returns>
        Task Delete(Guid id);

        /// <summary>
        /// Removes an user
        /// </summary>
        /// <param name="userIn">User to remove</param>
        Task Delete(User userIn);
    }
}
