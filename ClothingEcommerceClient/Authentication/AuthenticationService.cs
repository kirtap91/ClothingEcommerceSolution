using Blazored.LocalStorage;
using ClothingEcommerceClient.Services;
using ClothingEcommerceSharedLibrary.DTOs;
using ClothingEcommerceSharedLibrary.Responses;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Headers;
using System.Text;

namespace ClothingEcommerceClient.Authentication
{
    public class AuthenticationService(ILocalStorageService localStorageService, HttpClient httpClient)
    {
        public async Task<UserSession> GetUserDetail()
        {
            var token = await GetTokenFromLocalStorage();
            if (string.IsNullOrEmpty(token))
                return null!;

            var httpClient = await AddHeaderToHttpClient();
            var userDetails = JsonUtils.DeserializeJsonString<TokenProp>(token);
            if (userDetails is null || string.IsNullOrEmpty(userDetails.Token)) 
                return null!;

            var response = await GetUserDetailsFromApi();
            if (response.IsSuccessStatusCode)
                return await GetUserSession(response);

            if (httpClient.DefaultRequestHeaders.Contains("Authorization") &&
                response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                var encodedToken = Encoding.UTF8.GetBytes(userDetails.RefreshToken!);
                var validToken = WebEncoders.Base64UrlEncode(encodedToken);
                var model = new PostRefreshTokenDTO()
                {
                    RefreshToken = validToken
                };

                var result = await httpClient.PostAsync("api/account(refresh-token",
                    JsonUtils.GenerateStringContent(JsonUtils.SerializeObj(model)));
                if (result.IsSuccessStatusCode && result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var apiResponse = await result.Content.ReadAsStringAsync();
                    var loginResponse = JsonUtils.DeserializeJsonString<LoginResponse>(apiResponse);

                    await SetTokenToLocalStorage(JsonUtils.SerializeObj(new TokenProp() 
                    { 
                        Token = loginResponse.Token,
                        RefreshToken = loginResponse.RefreshToken
                    }));

                    var callApiAgain = await GetUserDetailsFromApi();
                    if (callApiAgain.IsSuccessStatusCode)
                    {
                        return await GetUserSession(callApiAgain);
                    }
                }
            }
            return null!;
        }
        public async Task SetTokenToLocalStorage(string token) =>
            await localStorageService.SetItemAsStringAsync("access_token", token);

        public static async Task<UserSession> GetUserSession(HttpResponseMessage response)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            return JsonUtils.DeserializeJsonString<UserSession>(apiResponse);
        }

        public async Task<HttpClient> AddHeaderToHttpClient()
        {
            httpClient.DefaultRequestHeaders.Remove("Authorization");
            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", 
                JsonUtils.DeserializeJsonString<TokenProp>(await GetTokenFromLocalStorage()).Token);
            return httpClient;
        }

        private async Task<string> GetTokenFromLocalStorage() => 
            await localStorageService.GetItemAsStringAsync("access_token");

        private async Task<HttpResponseMessage> GetUserDetailsFromApi()
        {
            var httpClient = await AddHeaderToHttpClient();
            return await httpClient.GetAsync("api/account/user-info");
        }
    }
}
