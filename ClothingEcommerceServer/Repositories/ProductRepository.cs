using ClothingEcommerceServer.Data;
using ClothingEcommerceSharedLibrary.Contracts;
using ClothingEcommerceSharedLibrary.Models;
using ClothingEcommerceSharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;

namespace ClothingEcommerceServer.Repositories
{
    public class ProductRepository(AppDbContext appDbContext) : IProduct
    {
        public async Task<ServiceResponse>  AddProduct(Product model)
        {
            if (model == null) return new ServiceResponse(false, "Model is null");
            var (flag, message) = await CheckName(model.Name!);
            if(flag)
            {
                appDbContext.Products.Add(model);
                await Commit();
                return new ServiceResponse(true, "Product Saved");
            }
            return new ServiceResponse(flag, message);
        }

        public async Task<ServiceResponse> AddProductVariant(int productId, ProductVariant variant)
        {
            var product = await appDbContext.Products.FindAsync(productId);
            if (product == null)
                return new ServiceResponse(false, "Product not found");

            product.Variants.Add(variant);
            await Commit();
            return new ServiceResponse(true, "Variant added successfully");
        }


        public async Task<List<Product>> GetAllProducts(bool featuredProducts)
        {
            if (featuredProducts)
            {
                return await appDbContext.Products
                                         .Where(_ => _.Featured)
                                         .Include(p => p.Images)  // Include images
                                         .Include(p => p.Variants)  // Optionally include variants if needed
                                         .ToListAsync();
            }
            else
            {
                return await appDbContext.Products
                                         .Include(p => p.Images)  // Include images
                                         .Include(p => p.Variants)  // Optionally include variants if needed
                                         .ToListAsync();
            }
        }

        public Task<Product> GetProductById(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ProductVariant>> GetProductVariantsByProductId(int productId)
        {
            return await appDbContext.ProductVariants
                                     .Where(v => v.ProductId == productId)
                                     .ToListAsync();
        }


        public async Task<ServiceResponse> RemoveProductVariant(int productId, int variantId)
        {
            var variant = await appDbContext.ProductVariants
                                            .FirstOrDefaultAsync(v => v.Id == variantId && v.ProductId == productId);
            if (variant == null)
                return new ServiceResponse(false, "Variant not found");

            appDbContext.ProductVariants.Remove(variant);
            await Commit();
            return new ServiceResponse(true, "Variant removed successfully");
        }


        public async Task<ServiceResponse> UpdateProductVariant(int productId, ProductVariant variant)
        {
            var existingVariant = await appDbContext.ProductVariants
                                                    .FirstOrDefaultAsync(v => v.Id == variant.Id && v.ProductId == productId);
            if (existingVariant == null)
                return new ServiceResponse(false, "Variant not found");

            // Update fields
            existingVariant.Size = variant.Size;
            existingVariant.Color = variant.Color;
            existingVariant.Quantity = variant.Quantity;
            existingVariant.Price = variant.Price;

            await Commit();
            return new ServiceResponse(true, "Variant updated successfully");
        }


        private async Task<ServiceResponse> CheckName(string name)
        {
            var product = await appDbContext.Products.FirstOrDefaultAsync(x => x.Name.ToLower()!.Equals(name.ToLower()));
            return product is null ? new ServiceResponse(true, null!) : new ServiceResponse(false, "Product already exist");
        }

        private async Task Commit() => await appDbContext.SaveChangesAsync();
    }
}
