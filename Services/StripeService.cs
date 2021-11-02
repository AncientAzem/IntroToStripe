using System;
using System.Collections.Generic;
using System.Linq;
using Stripe;
using Stripe.Checkout;
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

        public List<SessionLineItemOptions> GenerateSessionLineItems(bool onlySubscriptions = false)
        {
            var sessionCart = new List<SessionLineItemOptions>();
            // Get Data from Stripe
            var products = GetAllProducts();
            var productDataList = new List<ProductData>();
            foreach (var product in products)
            {
                var price = GetPriceForProduct(product).FirstOrDefault();
                productDataList.Add(new ProductData()
                {
                    Id = product.Id,
                    PrimaryPriceId = price?.Id,
                    PrimaryPriceValue = price?.UnitAmount,
                    Type = price?.Type ?? "one_time",
                    Name = product.Name,
                    ImageUrl = product.Images.First()
                });
            }
            
            if (onlySubscriptions)
            {
                foreach (var product in productDataList.Where(p => p.Type == "recurring"))
                {
                    sessionCart.Add(new SessionLineItemOptions()
                    {
                        Price = product.PrimaryPriceId,
                        Quantity = 1
                    });
                }
            }
            else
            {
                foreach (var product in productDataList.Where(p => p.Type == "one_time"))
                {
                    if (product.PrimaryPriceId is not null)
                    {
                        sessionCart.Add(new SessionLineItemOptions()
                        {
                            Price = product.PrimaryPriceId,
                            Quantity = 1
                        });
                    }
                    else
                    {
                        sessionCart.Add(new SessionLineItemOptions()
                        {
                            Quantity = 1,
                            PriceData = new SessionLineItemPriceDataOptions()
                            {
                                Currency = "USD",
                                Product = product.Id,
                                UnitAmount = 1000
                            }
                        });
                    }
                }
            }

            return sessionCart;
        }
    }
}