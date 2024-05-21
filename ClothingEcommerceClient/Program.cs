using Blazored.LocalStorage;
using ClothingEcommerceClient;
using ClothingEcommerceClient.Authentication;
using ClothingEcommerceClient.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Syncfusion.Blazor;

Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Mgo+DSMBMAY9C3t2UFhhQlJBfVldX2NWfFN0QXNYf1R0dl9DYEwgOX1dQl9nSXtRd0RiW3hfcnNcQWU=;Mgo+DSMBPh8sVXJxS0d+X1RPcEBAXXxLflFyVWJZdVpyflBGcC0sT3RfQFljTHxSd0RhXn5ceHVWRA==;MzI4NDU5NUAzMjM1MmUzMDJlMzBMdVVJQWl0S3RiTmtyZDhNazFMK3ZNUjF1RUZGTm5GOWNHZ09HZDRaam5nPQ==");
var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IProductService, ClientServices>();
builder.Services.AddScoped<ICategoryService, ClientServices>();
builder.Services.AddScoped<IUserAccountService, ClientServices>();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped <AuthenticationService>();
builder.Services.AddScoped<MessageDialogService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddSyncfusionBlazor();
await builder.Build().RunAsync();
