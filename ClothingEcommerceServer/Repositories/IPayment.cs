using ClothingEcommerceClient.PrivateModels;
using Stripe.Checkout;

namespace ClothingEcommerceServer.Repositories
{
    public interface IPayment
    {
        string CreateCheckoutSession(List<Order> cartItems);
    }
}
