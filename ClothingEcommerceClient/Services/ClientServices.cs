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
            UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip
        };

        private string SerializeObj(object modelObject) =>
            JsonSerializer.Serialize(modelObject, _jsonOptions);

        private T DeserializeJsonString<T>(string jsonString) =>
            JsonSerializer.Deserialize<T>(jsonString, _jsonOptions) ?? throw new JsonException("Deserialization failed.");

        private StringContent GenerateStringContent(string serializedObj) =>
            new StringContent(serializedObj, Encoding.UTF8, "application/json");

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
