using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PushNotificationsManagerAPI.Models
{
    /// <summary>
    /// Model for the notification messages.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Notification message title
        /// </summary> 
        [Key]
        [Required]
        public string Title{ get; set; }

        /// <summary>
        /// The full body of the notification message
        /// </summary>
        [Required]
        public string MessageBody { get; set; }

        /// <summary>
        /// Creation date of the notification message
        /// </summary>
        [Required]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// The creator of the notification message
        /// </summary>
        [Required]
        public string CreateBy { get; set; }

        /// <summary>
        /// Deep link action
        /// </summary>
        [Required]
        public string DeepLinkAction { get; set; }

        /// <summary>
        /// Importance level
        /// </summary>
        [Required]
        public int ImportanceLevel { get; set; }

        /// <summary>
        /// The creation User.
        /// </summary>
        [ForeignKey("CreateBy")]
        public User CreationUser { get; set; }
    }
}
