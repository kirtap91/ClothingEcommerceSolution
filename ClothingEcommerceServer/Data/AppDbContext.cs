using ClothingEcommerceSharedLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ClothingEcommerceServer.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        }
        public DbSet<Product> Products { get; set; } = default!;
    }
}
