using ClothingEcommerceSharedLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ClothingEcommerceServer.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<Category> Categories { get; set; } = default!;
    }
}
