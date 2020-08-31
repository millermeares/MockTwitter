using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
namespace MockTwitterAPI.Models
{
    public class TwitterContext : DbContext
    {
        public TwitterContext(DbContextOptions<TwitterContext> options) : base(options)
        {
        }
        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
