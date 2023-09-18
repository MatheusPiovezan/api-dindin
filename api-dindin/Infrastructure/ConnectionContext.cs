using api_dindin.Models;
using Microsoft.EntityFrameworkCore;

namespace api_dindin.Infrastructure
{
    public class ConnectionContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(
                "Server=localhost;" +
                "Port=5432;Database=dbdindin;" +
                "User Id=postgres;" +
                "Password=mfp45678;"
                );
        }
    }
}
