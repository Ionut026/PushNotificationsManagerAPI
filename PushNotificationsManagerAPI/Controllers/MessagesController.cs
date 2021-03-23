using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PushNotificationsManagerAPI.Models;
using PushNotificationsManagerAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PushNotificationsManagerAPI.Controllers
{
    /// <summary>
    /// Message Controller - provides endpoints for CRUD operations against messages.
    /// </summary>
    [Route("api/[controller]")]
    [Authorize(Roles = UserRoles.Admin + "," + UserRoles.User)]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MessagesController(IMessageRepository messageRepository, IHttpContextAccessor httpContextAccessor)
        {
            this._messageRepository = messageRepository;
            this._httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get all messages
        /// </summary>
        /// <returns>All messages</returns>
        [HttpGet]
        public async Task<IEnumerable<Message>> GetMessages()
        {
            string authenticatedUserRole = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role).Value;

            if (authenticatedUserRole.Equals(UserRoles.Admin))
            {
                return await _messageRepository.Get();
            }
            else
            {
                string authenticatedUser = _httpContextAccessor.HttpContext.User.Identity.Name;
                return await _messageRepository.GetByUser(authenticatedUser);
            }
        }

        /// <summary>
        /// Gets the message by title
        /// </summary>
        /// <param name="title"></param>
        /// <returns>The found message for the provided title.</returns>
        [HttpGet("{title}")]
        public async Task<ActionResult<Message>> GetMessages(string title)
        {
            return await _messageRepository.Get(title);
        }

        /// <summary>
        /// Creates a new notification message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>Returns the newly created message</returns>
        [HttpPost]
        public async Task<ActionResult<Message>> PostMessages([FromBody] Message message)
        {
            try
            {
                var newMessage = await _messageRepository.Create(message);
                return CreatedAtAction(nameof(PostMessages), new { title = newMessage.Title }, newMessage);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException != null ? ex.Message + " Inner exception: " + ex.InnerException.Message : ex.Message);
            }
        }

        /// <summary>
        /// Updates a message
        /// </summary>
        /// <param name="message"></param>
        /// <returns>The updated message</returns>
        [HttpPut]
        public async Task<ActionResult<Message>> PutMessages(string title, [FromBody] Message message)
        {
            if (!title.Equals(message.Title))
            {
                return BadRequest();
            }

            var updatedMessage = await _messageRepository.Update(message);

            return CreatedAtAction(nameof(PutMessages), new { title = updatedMessage.Title }, updatedMessage);
        }

        /// <summary>
        /// Delete a message by title.
        /// </summary>
        /// <param name="title"></param>
        /// <returns>The operation result</returns>
        [HttpDelete("{title}")]
        public async Task<ActionResult> Delete(string title)
        {
            var messageToDelete = await _messageRepository.Get(title);
            if (messageToDelete == null)
                return NotFound();

            await _messageRepository.Delete(messageToDelete.Title);
            return NoContent();
        }
    }
}
