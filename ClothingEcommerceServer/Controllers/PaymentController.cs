using ClothingEcommerceClient.PrivateModels;
using ClothingEcommerceServer.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClothingEcommerceServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(IPayment paymentService) : ControllerBase
    {
        [HttpPost("checkout")]
        public ActionResult CreateCheckoutSession(List<Order> cartItems)
        {
            var url = paymentService.CreateCheckoutSession(cartItems);
            return Ok(url);
        }
    }
}
