using ClothingEcommerceSharedLibrary.Models;
using ClothingEcommerceSharedLibrary.Responses;

namespace ClothingEcommerceSharedLibrary.Contracts
{
    public interface IProduct
    {
        Task<ServiceResponse> AddProduct(Product model);
    }
}
