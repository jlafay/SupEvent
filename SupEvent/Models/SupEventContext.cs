using System.Data.Entity;

namespace _SupEvent.Models
{
    public class SupEventContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Guest> Guests { get; set; }
    }
}