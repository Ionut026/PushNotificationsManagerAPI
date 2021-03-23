using Microsoft.EntityFrameworkCore;
using PushNotificationsManagerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace PushNotificationsManagerAPI.Repositories
{
    /// <summary>
    /// Implements all actions for the notification messages
    /// </summary>
    public class MessageRepository : IMessageRepository
    {
        private readonly ManagerDbContext _context;

        public MessageRepository(ManagerDbContext context)
        {
            this._context = context;
        }

        /// <summary>
        /// Creates a new notification message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Returns the newly created message</returns>
        public async Task<Message> Create(Message message)
        {
            message.CreateDate = System.DateTime.Now;
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
            return await Get(message.Title);
        }

        /// <summary>
        /// Delete a message by title.
        /// </summary>
        /// <param name="title"></param>
        /// <returns>The operation result</returns>
        public async Task Delete(string title)
        {
            var messageToDelete = await _context.Messages.FindAsync(title);
            _context.Messages.Remove(messageToDelete);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Get all messages
        /// </summary>
        /// <returns>All messages</returns>
        public async Task<IEnumerable<Message>> Get()
        {
            return await _context.Messages.Include(message => message.CreationUser).ToListAsync();
        }

        /// <summary>
        /// Gets the message by title
        /// </summary>
        /// <param name="title"></param>
        /// <returns>The found message for the provided title.</returns>
        public async Task<Message> Get(string title)
        {
            return await _context.Messages.Include(message => message.CreationUser).FirstOrDefaultAsync(message => message.Title.Equals(title));
        }

        /// <summary>
        /// Updates a message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>The updated message</returns>
        public async Task<Message> Update(Message message)
        {
            _context.Entry(message).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return await Get(message.Title);
        }

        /// <summary>
        /// Gets the message by user
        /// </summary>
        /// <param name="userName">The username</param>
        /// <returns>The found message for the provided username.</returns>
        public async Task<IEnumerable<Message>>  GetByUser(string userName)
        {
            return await _context.Messages.Where(x => x.CreateBy.Equals(userName)).Include(message => message.CreationUser).ToListAsync();
        }
    }
}
