using Microsoft.EntityFrameworkCore;
using PushNotificationsManagerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PushNotificationsManagerAPI.Repositories
{
    /// <summary>
    /// Implements all actions for the users repository.
    /// </summary>
    class UserRepository : IUserRepository
    {
        private readonly ManagerDbContext _context;
        
        public UserRepository(ManagerDbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The newly created user.</returns>
        public async Task<User> Create(User user)
        {      
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return await Get(user.UserName);
        }

        /// <summary>
        /// Remove the user by userName
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>The operation result</returns>
        public async Task Delete(string userName)
        {
            var userToDelete = await _context.Users.FindAsync(userName);
            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>All available users</returns>
        public async Task<IEnumerable<User>> Get()
        {
            return await _context.Users.ToListAsync();
        }

        /// <summary>
        /// Gets an user by the userName.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>The found user.</returns>
        public async Task<User> Get(string userName)
        {
            return await _context.Users.FindAsync(userName);
        }

        /// <summary>
        /// Updates an user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>The updated user.</returns>
        public async Task<User> Update(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return await Get(user.UserName);
        }
    }
}
