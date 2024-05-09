using ClothingEcommerceSharedLibrary.Models;
using ClothingEcommerceSharedLibrary.Responses;

namespace ClothingEcommerceClient.Services
{
    public interface ICategoryService
    {
        Action? CategoryAction { get; set; }
        Task<ServiceResponse> AddCategory(Category model);
        Task GetAllCategories();
        List<Category> AllCategories { get; set; }
    }
}
