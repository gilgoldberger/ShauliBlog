using ClassExe3.Models;
using System.Data.Entity;

namespace ClassExe3.DAL
{
    public class ShauliBlogContext : DbContext
    {
        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Fan> Fans { get; set; }
        public DbSet<Account> Accounts { get; set; }
    }
}