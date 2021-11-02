using System;
using System.Collections.Generic;
using Stripe;
using StripeDemo.Models;

namespace StripeDemo.Services
{
    public class StripeService
    {
        public StripeList<Product> GetAllProducts()
        {
            var productService = new ProductService();
            StripeList<Product> products = productService.List();
            Console.Write(products);
            return products;
        }
    }
}