using ClothingEcommerceSharedLibrary.DTOs;
using ClothingEcommerceSharedLibrary.Models;
using ClothingEcommerceSharedLibrary.Responses;

namespace ClothingEcommerceClient.Services
{
    public class ClientServices(HttpClient httpClient) : IProductService, ICategoryService, IUserAccountService
    {
        private const string ProductBaseUrl = "api/product";
        private const string CategoryBaseUrl = "api/category";
        private const string AuthenticationBaseUrl = "api/account";

        public Action? CategoryAction { get; set; }
        public List<Category> AllCategories { get; set; }
        public Action? ProductAction { get; set; }
        public List<Product> AllProducts { get; set; }
        public List<Product> FeaturedProducts { get; set; }
        public List<Product> ProductsByCategory { get ; set ; }
        public bool IsVisible { get; set; }


        //Products
        public async Task<ServiceResponse> AddProduct(Product model)
        {
            var response = await httpClient.PostAsync(ProductBaseUrl, JsonUtils.GenerateStringContent(JsonUtils.SerializeObj(model)));

            //Read Response
            var result = CheckResponse(response);
            if (!result.IsSuccessful)
            {
                return result;
            }

            var apiResponse = await ReadContent(response);
            var data = JsonUtils.DeserializeJsonString<ServiceResponse>(apiResponse);
            if (!data.IsSuccessful) return data;
            await ClearAndGetAllProducts();
            return data;
        }

        private async Task ClearAndGetAllProducts()
        {
            bool featuredProduct = true;
            bool allProduct = false;
            AllProducts = null!;
            FeaturedProducts = null!;
            await GetAllProducts(featuredProduct);
            await GetAllProducts(allProduct);
        }
        public async Task GetAllProducts(bool featuredProducts)
        {
            if(featuredProducts && FeaturedProducts is null)
            {
                IsVisible = true;
                FeaturedProducts = await GetProducts(featuredProducts);
                IsVisible = false;
                ProductAction?.Invoke();
                return;
            }
            else
            {
                if (!featuredProducts && AllProducts is null)
                {
                    IsVisible = true;
                    AllProducts = await GetProducts(featuredProducts);
                    IsVisible = false;
                    ProductAction?.Invoke();
                    return;
                }
            }

        }
        private async Task<List<Product>> GetProducts(bool featured)
        {
            var response = await httpClient.GetAsync($"{ProductBaseUrl}?featured={featured}");
            var isSuccessful = CheckResponse(response);
            if (!isSuccessful.IsSuccessful)
            {
                return null!;
            }
            var result = await ReadContent(response);
            return (List<Product>?)JsonUtils.DeserializeJsonStringList<Product>(result)!;
        }

        public async Task GetProductsByCategory(int categoryId)
        {
            bool featured = false;
            await GetAllProducts(featured);
            ProductsByCategory = AllProducts.Where(_ => _.CategoryId == categoryId).ToList();
            ProductAction?.Invoke();

        }

        public Product GetRandomFeaturedProduct()
        {
            if (FeaturedProducts == null || FeaturedProducts.Count == 0)
                return null!;

            Random randomNumbers = new();
            int index = randomNumbers.Next(0, FeaturedProducts.Count);
            return FeaturedProducts[index];
        }



        //Categories

        public async Task<ServiceResponse> AddCategory(Category model)
        {
            var response = await httpClient.PostAsync(CategoryBaseUrl, JsonUtils.GenerateStringContent(JsonUtils.SerializeObj(model)));

            //Read Response
            var result = CheckResponse(response);
            if (!result.IsSuccessful)
            {
                return result;
            }

            var apiResponse = await ReadContent(response);
            
            
            var data = JsonUtils.DeserializeJsonString<ServiceResponse>(apiResponse);
            if (!data.IsSuccessful) return data;
            await ClearAndGetAllCategories();
            return data;

        }

        public async Task GetAllCategories()
        {
            if (AllCategories is null)
            {
                var response = await httpClient.GetAsync($"{CategoryBaseUrl}");
                var isSuccessful = CheckResponse(response);
                if (!isSuccessful.IsSuccessful)
                {
                    return;
                }

                var result = await ReadContent(response);
                AllCategories = (List<Category>?)JsonUtils.DeserializeJsonStringList<Category>(result)!;
                CategoryAction?.Invoke();
            }
        }
        private async Task ClearAndGetAllCategories()
        {
            AllCategories = null!;
            await GetAllCategories();
        }


        //General Method

        private static ServiceResponse CheckResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                return new ServiceResponse(false, "Error occured. Try again later...");
            else
                return new ServiceResponse(true, null!);
        }
        private static async Task<string> ReadContent(HttpResponseMessage response) => await response.Content.ReadAsStringAsync();

        //Account/authentication
        public async Task<ServiceResponse> Register(UserDTO model)
        {
            var response = await httpClient.PostAsync($"{AuthenticationBaseUrl}/register",
                JsonUtils.GenerateStringContent(JsonUtils.SerializeObj(model)));
            var result = CheckResponse(response);
            if (!result.IsSuccessful)
                return result;

            var apiResponse = await ReadContent(response);
            return JsonUtils.DeserializeJsonString<ServiceResponse>(apiResponse);
        }

        public async Task<LoginResponse> Login(LoginDTO model)
        {
            var response = await httpClient.PostAsync($"{AuthenticationBaseUrl}/login",
                JsonUtils.GenerateStringContent(JsonUtils.SerializeObj(model)));
            if (!response.IsSuccessStatusCode)
                return new LoginResponse(false, "Error occured", null!, null!);

            var apiResponse = await ReadContent(response);
            return JsonUtils.DeserializeJsonString<LoginResponse>(apiResponse);
        }
    }
}
