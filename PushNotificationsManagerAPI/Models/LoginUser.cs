using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PushNotificationsManagerAPI.Models
{
    /// <summary>
    /// The user model for the login request
    /// </summary>
    public class LoginUser
    {
        /// <summary>
        /// The user name
        /// </summary>
        public string UserName
        {
            get; set;
        }

        /// <summary>
        /// The password
        /// </summary>
        public string Password
        {
            get; set;
        }
    }
}
