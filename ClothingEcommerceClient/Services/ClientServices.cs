﻿using ClothingEcommerceSharedLibrary.Models;
using ClothingEcommerceSharedLibrary.Responses;

namespace ClothingEcommerceClient.Services
{
    public class ClientServices(HttpClient httpClient) : IProductService, ICategoryService
    {
        private const string ProductBaseUrl = "api/product";
        private const string CategoryBaseUrl = "api/category";

        public Action? CategoryAction { get; set; }
        public List<Category> AllCategories { get; set; }
        public Action? ProductAction { get; set; }
        public List<Product> AllProducts { get; set; }
        public List<Product> FeaturedProducts { get; set; }

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
            await ClearAndGetAllProducts();
            {
                return JsonUtils.DeserializeJsonString<ServiceResponse>(apiResponse);
            }
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
                FeaturedProducts = await GetProducts(featuredProducts);
                ProductAction?.Invoke();
                return;
            }
            else
            {
                if (!featuredProducts && AllProducts is null)
                {
                    AllProducts = await GetProducts(featuredProducts);
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
            await ClearAndGetAllCategories();
            {
                return JsonUtils.DeserializeJsonString<ServiceResponse>(apiResponse);
            }
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

        private static ServiceResponse CheckResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                return new ServiceResponse(false, "Error occured. Try again later...");
            else
                return new ServiceResponse(true, null!);
        }
        private static async Task<string> ReadContent(HttpResponseMessage response) => await response.Content.ReadAsStringAsync();
    }
}
