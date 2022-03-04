using Microsoft.EntityFrameworkCore;
using RateSetter.Models;

namespace RateSetter.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<User> Users { get; set; }

    }
}