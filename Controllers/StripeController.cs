using System.Collections.Generic;
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
            ViewData["products-json"] = _stripe.GetAllProducts().ToJson();
            return View();
        }

        [HttpPost]
        public IActionResult StartCheckout()
        {
            return null;
        }
    }
}