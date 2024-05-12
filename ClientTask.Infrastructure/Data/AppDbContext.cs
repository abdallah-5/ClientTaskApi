
using System.Reflection;
using ClientTask.Core.Models;
using ClientTask.Infrastructure.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace ClientTask.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<Trade> Trades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClientConfiguration());
        }
    }
}