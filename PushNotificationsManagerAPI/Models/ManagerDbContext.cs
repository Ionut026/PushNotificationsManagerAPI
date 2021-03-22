using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PushNotificationsManagerAPI.Models
{
    public class ManagerDbContext : DbContext
    {
        /// <summary>
        /// Constructor for ManagerDbContext.
        /// </summary>
        /// <param name="options"></param>
        public ManagerDbContext(DbContextOptions<ManagerDbContext> options) : base(options)
        {
            // Create the database if it does not exist.
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Message> Messages { get; set; }

    }
}
