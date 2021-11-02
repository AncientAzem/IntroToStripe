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
            return new ProductService().List();
        }

        public StripeList<Price> GetPriceForProduct(Product item)
        {
            var options = new PriceListOptions()
            {
                Product = item.Id
            };
            return new PriceService().List(options);
        }
    }
}