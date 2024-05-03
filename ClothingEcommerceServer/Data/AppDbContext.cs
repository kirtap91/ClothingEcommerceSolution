using ClothingEcommerceSharedLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace ClothingEcommerceServer.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; } = default!;
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Product to ProductVariants
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Variants)
                .WithOne(v => v.Product)
                .HasForeignKey(v => v.ProductId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete on ProductVariant when Product is deleted

            // Product to ProductImages
            modelBuilder.Entity<Product>()
                .HasMany(p => p.Images)
                .WithOne(img => img.Product)
                .HasForeignKey(img => img.ProductId)
                .OnDelete(DeleteBehavior.Cascade);  // Cascade delete on ProductImage when Product is deleted

            // ProductVariant to ProductImages
            modelBuilder.Entity<ProductVariant>()
                .HasMany(pv => pv.Images)
                .WithOne(img => img.ProductVariant)
                .HasForeignKey(img => img.ProductVariantId)
                .OnDelete(DeleteBehavior.NoAction);  // Set ProductVariantId in ProductImage to null when ProductVariant is deleted
        }



    }


}
