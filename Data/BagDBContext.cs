using Microsoft.EntityFrameworkCore;
using BagAPI.Models;

namespace BagAPI.Data
{
    public class BagDBContext : DbContext
    {
        public BagDBContext(DbContextOptions<BagDBContext> options) : base(options) { }

        public DbSet<Users> Users { get; set; }  

        public DbSet<Products> Products { get; set; }

        public DbSet<Roles> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().ToTable("Users");
            modelBuilder.Entity<Products>().ToTable("Products");
            modelBuilder.Entity<Roles>().ToTable("Roles");
            
        }

    
    }
}
