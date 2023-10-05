using api_dindin.Models;
using Microsoft.EntityFrameworkCore;

namespace api_dindin.Context
{
    public class DbConnectionContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        public DbConnectionContext(DbContextOptions<DbConnectionContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}
