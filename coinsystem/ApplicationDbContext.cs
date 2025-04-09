// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using coinsystem.Models; // อย่าลืม using Model ของคุณ

namespace coinsystem
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // ระบุ DbSet ของคุณ เช่น Money
        public DbSet<Money> Money { get; set; }
        public DbSet<Product> Product { get; set; }
        
        public DbSet<Member> Member { get; set; }
    }
}