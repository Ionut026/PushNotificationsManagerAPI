using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PushNotificationsManagerAPI.Models
{
    /// <summary>
    /// Model for users.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The user name
        /// </summary>
        [Key]
        [Required]
        public string UserName
        {
            get; set;
        }

        /// <summary>
        /// The user password
        /// </summary>
        [Required]
        public string Password
        {
            get; set;
        }

        /// <summary>
        /// Role
        /// </summary>
        [Required]
        public string Role
        {
            get; set;
        }
        
    }
}
