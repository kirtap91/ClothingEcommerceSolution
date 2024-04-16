using ClothingEcommerceSharedLibrary.Contracts;
using ClothingEcommerceSharedLibrary.Models;
using ClothingEcommerceSharedLibrary.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ClothingEcommerceServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProduct productService;

        public ProductController(IProduct productService)
        {
            this.productService = productService;
        }
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts(bool featured)
        {
            var products = await productService.GetAllProducts(featured); return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> AddProduct(Product model)
        {
            if(model == null) return BadRequest(ModelState);
            var response = await productService.AddProduct(model);
            return Ok(response);
        }
    }
}
