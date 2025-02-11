using ClothingEcommerceServer.Repositories;
using ClothingEcommerceSharedLibrary.Models;
using ClothingEcommerceSharedLibrary.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ClothingEcommerceServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(ICategory categoryService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            var products = await categoryService.GetAllCategories();
            return Ok(products); 
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse>> AddCategory(Category model)
        {
            if(model is null) return BadRequest(ModelState);
            var response = await categoryService.AddCategory(model);
            return Ok(response);
        }
    }
}
