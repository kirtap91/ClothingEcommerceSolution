using ClothingEcommerceClient.PrivateModels;
using Stripe;
using Stripe.Checkout;

namespace ClothingEcommerceServer.Repositories
{
    public class PaymentRepository : IPayment
    {
        public PaymentRepository()
        {
            StripeConfiguration.ApiKey = "sk_test_51QexD1RfpUnyO2nHP7OjeDh6OxC7pabw3GZLiBT2WuzAnXTYrgQ8Kvl9IdEC6qqQdA0le6TXc5ixJcg5U1mBIfCO00OqXAedAd";
        }
        public string CreateCheckoutSession(List<Order> cartItems)
        {
            if (cartItems is null)
                return null!;

            var lineItems = new List<SessionLineItemOptions>();
            cartItems.ForEach(item =>
            {
                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "sek",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Name,
                            Description = item.Id.ToString()
                        },
                        UnitAmountDecimal = (long)(item.Price * 100)
                    },
                    Quantity = item.Quantity
                });
            });

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes =
                [
                    "card"
                ],
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = "https://localhost:7190/order-success",
                CancelUrl = "https://localhost:7190/cancel"
            };

            var service = new SessionService();
            Session session = service.Create(options);
            return session.Url;

        }
    }
}