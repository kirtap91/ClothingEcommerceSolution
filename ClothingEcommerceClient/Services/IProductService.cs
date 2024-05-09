﻿using ClothingEcommerceSharedLibrary.Models;
using ClothingEcommerceSharedLibrary.Responses;

namespace ClothingEcommerceClient.Services
{
    public interface IProductService
    {
        Action? ProductAction { get; set; } 
        Task<ServiceResponse> AddProduct(Product model);
        Task GetAllProducts(bool featuredProducts);
        List<Product> AllProducts { get; set; }
    }

}
