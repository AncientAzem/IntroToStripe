using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public IActionResult StartCheckout()
        {
            return null;
        }
    }
}