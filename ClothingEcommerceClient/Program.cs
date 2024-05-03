using ClothingEcommerceClient;
using ClothingEcommerceClient.Pages;
using ClothingEcommerceClient.Services;
using ClothingEcommerceSharedLibrary.Contracts;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IProduct, ClientServices>();


await builder.Build().RunAsync();
