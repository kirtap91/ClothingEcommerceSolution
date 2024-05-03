using ClothingEcommerceSharedLibrary.Models;
using ClothingEcommerceSharedLibrary.Responses;

namespace ClothingEcommerceSharedLibrary.Contracts
{
    public interface IProduct
    {
        Task<ServiceResponse> AddProduct(Product model);
        Task<ServiceResponse> AddProductVariant(int productId, ProductVariant variant);
        Task<ServiceResponse> UpdateProductVariant(int productId, ProductVariant variant);
        Task<Product> GetProductById(int productId);
        Task<List<Product>> GetAllProducts(bool featuredProducts);
        Task<List<ProductVariant>> GetProductVariantsByProductId(int productId);
        Task<ServiceResponse> RemoveProductVariant(int productId, int variantId);
    }
}

