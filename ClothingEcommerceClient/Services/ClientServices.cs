using ClothingEcommerceSharedLibrary.Contracts;
using ClothingEcommerceSharedLibrary.Models;
using ClothingEcommerceSharedLibrary.Responses;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClothingEcommerceClient.Services
{
    public class ClientServices(HttpClient httpClient) : IProduct
    {
        private const string BaseUrl = "api/product";
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            AllowTrailingCommas = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve,
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
        };

        private string SerializeObj(object modelObject) =>
            JsonSerializer.Serialize(modelObject, _jsonOptions);

        private T DeserializeJsonString<T>(string jsonString) =>
            JsonSerializer.Deserialize<T>(jsonString, _jsonOptions) ?? throw new JsonException("Deserialization failed.");

        private StringContent GenerateStringContent(string serializedObj) =>
            new(serializedObj, Encoding.UTF8, "application/json");

        public async Task<ServiceResponse> AddProduct(Product model)
        {
            var response = await httpClient.PostAsync(BaseUrl, GenerateStringContent(SerializeObj(model)));
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Error occurred. Try again later...");
            }

            var apiResponse = await response.Content.ReadAsStringAsync();
            return DeserializeJsonString<ServiceResponse>(apiResponse);
        }

        public async Task<List<Product>> GetAllProducts(bool featuredProducts)
        {
            var response = await httpClient.GetAsync($"{BaseUrl}?featured={featuredProducts}");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Error occurred. Could not retrieve products.");
            }

            var result = await response.Content.ReadAsStringAsync();
            return DeserializeJsonString<List<Product>>(result);
        }

        public async Task<ServiceResponse> AddProductVariant(int productId, ProductVariant variant)
        {
            var response = await httpClient.PostAsync($"{BaseUrl}/{productId}/variants", GenerateStringContent(SerializeObj(variant)));
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Error adding product variant.");
            }

            var apiResponse = await response.Content.ReadAsStringAsync();
            return DeserializeJsonString<ServiceResponse>(apiResponse);
        }

        public async Task<ServiceResponse> UpdateProductVariant(int productId, ProductVariant variant)
        {
            var response = await httpClient.PutAsync($"{BaseUrl}/{productId}/variants/{variant.Id}", GenerateStringContent(SerializeObj(variant)));
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Error updating product variant.");
            }

            var apiResponse = await response.Content.ReadAsStringAsync();
            return DeserializeJsonString<ServiceResponse>(apiResponse);
        }

        public async Task<Product> GetProductById(int productId)
        {
            var response = await httpClient.GetAsync($"{BaseUrl}/{productId}");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Error retrieving product details.");
            }

            var result = await response.Content.ReadAsStringAsync();
            return DeserializeJsonString<Product>(result);
        }

        public async Task<List<ProductVariant>> GetProductVariantsByProductId(int productId)
        {
            var response = await httpClient.GetAsync($"{BaseUrl}/{productId}/variants");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Error retrieving product variants.");
            }

            var result = await response.Content.ReadAsStringAsync();
            return DeserializeJsonString<List<ProductVariant>>(result);
        }


        public async Task<ServiceResponse> RemoveProductVariant(int productId, int variantId)
        {
            var response = await httpClient.DeleteAsync($"{BaseUrl}/{productId}/variants/{variantId}");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Error removing product variant.");
            }

            var apiResponse = await response.Content.ReadAsStringAsync();
            return DeserializeJsonString<ServiceResponse>(apiResponse);
        }
    }
    //private static string SerializeObj(object modelObject) =>
    //    JsonSerializer.Serialize(modelObject, JsonOptions());
    //    private static T DeserializeJsonString<T>(string jsonString) => JsonSerializer.Deserialize<T>(jsonString, JsonOptions())!;
    //    private static StringContent GenerateStringContent(string serializedObj) => new(serializedObj, System.Text.Encoding.UTF8, "application/json"); 
    //    private static IList<T> DeserializeJsonStringList<T>(string jsonString) => JsonSerializer.Deserialize<IList<T>>(jsonString, JsonOptions())!;
    //    private static JsonSerializerOptions JsonOptions()
    //    {
    //        return new JsonSerializerOptions
    //        {
    //            AllowTrailingCommas = true,
    //            PropertyNameCaseInsensitive = true,
    //            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    //            UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
    //        };
    //    }


    //    public async Task<ServiceResponse> AddProduct(Product model)
    //    {
    //        var response = await httpClient.PostAsync(BaseUrl, GenerateStringContent(SerializeObj(model)));

    //        if (!response.IsSuccessStatusCode)
    //            return new ServiceResponse(false, "Error ocured. Try again later...");

    //        var apiResponse = await response.Content.ReadAsStringAsync();
    //            return DeserializeJsonString<ServiceResponse>(apiResponse);
    //    }

    //    public async Task<List<Product>> GetAllProducts(bool featuredProducts)
    //    {
    //        var response = await httpClient.GetAsync($"{BaseUrl}?featured={featuredProducts}");
    //        if (!response.IsSuccessStatusCode) return null!;

    //        var result = await response.Content.ReadAsStringAsync();
    //        return [.. DeserializeJsonStringList<Product>(result)];
    //    }
    //}
}
