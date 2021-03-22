using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PushNotificationsManagerAPI.Models;

namespace PushNotificationsManagerAPI.Repositories
{
    /// <summary>
    /// IUserRepository - defines all actions for the users repository.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>All available users</returns>
        Task<IEnumerable<User>> Get();

        /// <summary>
        /// Gets an user by the userName.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>The found user.</returns>
        Task<User> Get(string userName);

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The newly created user.</returns>
        Task<User> Create(User user);

        /// <summary>
        /// Updates an user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The updated user.</returns>
        Task<User> Update(User user);

        /// <summary>
        /// Remove the user by userName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>The operation result</returns>
        Task Delete(string userName);
    }
}
