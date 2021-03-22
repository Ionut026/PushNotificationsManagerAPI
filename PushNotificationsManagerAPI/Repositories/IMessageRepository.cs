using PushNotificationsManagerAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PushNotificationsManagerAPI.Repositories
{
    /// <summary>
    /// IMessageRepository - defines all actions for the notification messages
    /// </summary>
    public interface IMessageRepository
    {
        /// <summary>
        /// Get all messages
        /// </summary>
        /// <returns>All messages</returns>
        Task<IEnumerable<Message>> Get();

        /// <summary>
        /// Gets the message by title
        /// </summary>
        /// <param name="title"></param>
        /// <returns>The found message for the provided title.</returns>
        Task<Message> Get(string title);

        /// <summary>
        /// Creates a new notification message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Returns the newly created message</returns>
        Task<Message> Create(Message message);

        /// <summary>
        /// Updates a message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>The updated message</returns>
        Task<Message> Update(Message message);

        /// <summary>
        /// Delete a message by title.
        /// </summary>
        /// <param name="title"></param>
        /// <returns>The operation result</returns>
        Task Delete(string title);
    }
}
