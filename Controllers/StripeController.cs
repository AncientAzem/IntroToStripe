using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using StripeDemo.Models;
using StripeDemo.Services;

namespace StripeDemo.Controllers
{
    public class StripeController : Controller
    {
        private StripeService _stripe = new StripeService();

        // GET
        public IActionResult Index()
        {
            var productDataList = new List<ProductData>();
            
            // Get Data from Stripe
            var products = _stripe.GetAllProducts();
            foreach (var product in products)
            {
                var prices = _stripe.GetPriceForProduct(product).FirstOrDefault();
                productDataList.Add(new ProductData()
                {
                    Id = product.Id,
                    PrimaryPriceId = prices?.Id,
                    PrimaryPriceValue = prices?.UnitAmount,
                    Name = product.Name,
                    ImageUrl = product.Images.First()
                });
            }
            
            ViewData["products-json"] = products.ToJson();
            return View(productDataList);
        }
        
        public IActionResult StartCheckout(bool isSubscription = false)
        {
            var sessionOptions = new SessionCreateOptions()
            {
                LineItems = _stripe.GenerateSessionLineItems(isSubscription),
                PaymentMethodTypes = new List<string>() { "card" },
                Mode = isSubscription ? "subscription" : "payment",
                SuccessUrl = "https://google.com/",
                CancelUrl = "https://localhost:5001/Stripe"
            };

            Session session = new SessionService().Create(sessionOptions);
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }
    }
}