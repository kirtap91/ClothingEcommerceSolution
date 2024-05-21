using Blazored.LocalStorage;
using ClothingEcommerceClient.Authentication;
using ClothingEcommerceClient.PrivateModels;
using ClothingEcommerceSharedLibrary.DTOs;
using ClothingEcommerceSharedLibrary.Models;
using ClothingEcommerceSharedLibrary.Responses;

namespace ClothingEcommerceClient.Services
{
    public class ClientServices(HttpClient httpClient, AuthenticationService authenticationService, ILocalStorageService localStorageService) : IProductService, ICategoryService, IUserAccountService, ICart
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
        public Action? CartAction { get; set ; }
        public int CartCount { get ; set ; }
        public bool IsCartLoaderVisible { get; set; }


        //Products
        public async Task<ServiceResponse> AddProduct(Product model)
        {
            await authenticationService.GetUserDetails();
            var privateHttpClient = await authenticationService.AddHeaderToHttpClient();
            var response = await privateHttpClient.PostAsync(ProductBaseUrl, JsonUtils.GenerateStringContent(JsonUtils.SerializeObj(model)));

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
            await authenticationService.GetUserDetails();
            var privateHttpClient = await authenticationService.AddHeaderToHttpClient();
            var response = await privateHttpClient.PostAsync(CategoryBaseUrl, JsonUtils.GenerateStringContent(JsonUtils.SerializeObj(model)));

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

        //Account/authentication service
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


        // Cart Service
        public async Task GetCartCount()
        {
            string cartString = await GetCartFromLocalStorage();
            if (string.IsNullOrEmpty(cartString))
                CartCount = 0;
            else
                CartCount = JsonUtils.DeserializeJsonStringList<StorageCart>(cartString).Count;

            CartAction?.Invoke();
        }

        public async Task<ServiceResponse> AddToCart(Product model, int updateQuantity = 1)
        {
            string message = string.Empty;
            var myCart = new List<StorageCart>();
            var getCartFromStorage = await GetCartFromLocalStorage();
            if (!string.IsNullOrEmpty(getCartFromStorage))
            {
                myCart = (List<StorageCart>)JsonUtils.DeserializeJsonStringList<StorageCart>(getCartFromStorage);
                var checkIfAddedAlready = myCart.FirstOrDefault(_ => _.ProductId == model.Id);
                if (checkIfAddedAlready is null)
                {
                    myCart.Add(new StorageCart()
                    {
                        ProductId = model.Id,
                        Quantity = 1
                    });
                    message = "Product added to Cart";
                }
                else 
                {
                    var updateProduct = new StorageCart()
                    {
                        ProductId = model.Id,
                        Quantity = updateQuantity
                    };
                    myCart.Remove(checkIfAddedAlready!);
                    myCart.Add(updateProduct);
                    message = "Product Updated";
                }
            }
            else
            {
                myCart.Add(new StorageCart()
                {
                    Quantity = 1,
                    ProductId = model.Id
                });
                message = "Product Added to Cart";
            }
            await RemoveCartFromLocalStorage();
            await SetCartToLocalStorage(JsonUtils.SerializeObj(myCart));
            await GetCartCount();
            return new ServiceResponse(true, message);
        }

        public async Task<List<Order>> MyOrders()
        {
            IsCartLoaderVisible = true;
            var cartList = new List<Order>();
            string myCartString = await GetCartFromLocalStorage();
            if (string.IsNullOrEmpty(myCartString)) return null!;

            var myCartList = JsonUtils.DeserializeJsonStringList<StorageCart>(myCartString);
            await GetAllProducts(false);
            foreach(var cartItem in myCartList)
            {
                var product = AllProducts.FirstOrDefault(_ => _.Id == cartItem.ProductId);
                cartList.Add(new Order()
                {
                    Id = product!.Id,
                    Name = product.Name,
                    Quantity = cartItem.Quantity,
                    Price = product.Price,
                    Image = product.Base64Img
                });
            }
            IsCartLoaderVisible = false;
            await GetCartCount();
            return cartList;
        }

        public async Task<ServiceResponse> DeleteCart(Order cart)
        {
            var myCartList = JsonUtils.DeserializeJsonStringList<StorageCart>(await GetCartFromLocalStorage());
            if (myCartList is null)
            {
                return new ServiceResponse(false, "Product not found");                
            }
            myCartList.Remove(myCartList.FirstOrDefault(_ => _.ProductId == cart.Id)!);
            await RemoveCartFromLocalStorage();
            await SetCartToLocalStorage(JsonUtils.SerializeObj(myCartList));
            await GetCartCount();
            return new ServiceResponse(true, "Product removed successfully");
        }

        private async Task<string> GetCartFromLocalStorage() => await localStorageService.GetItemAsStringAsync("cart");
        private async Task SetCartToLocalStorage(string cart) => await localStorageService.SetItemAsStringAsync("cart", cart);
        private async Task RemoveCartFromLocalStorage() => await localStorageService.RemoveItemAsync("cart");
    }
}
