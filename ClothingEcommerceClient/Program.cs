using Blazored.LocalStorage;
using ClothingEcommerceClient;
using ClothingEcommerceClient.Authentication;
using ClothingEcommerceClient.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NCaF1cVGhIfEx1RHxQdld5ZFRHallYTnNWUj0eQnxTdEBjWH9WcHZRRWRbV0ZyXQ==");
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IProductService, ClientServices>();
builder.Services.AddScoped<ICategoryService, ClientServices>();
builder.Services.AddScoped<IUserAccountService, ClientServices>();
builder.Services.AddScoped<ICart, ClientServices>();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped <AuthenticationService>();
builder.Services.AddScoped<MessageDialogService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSyncfusionBlazor();
await builder.Build().RunAsync();
