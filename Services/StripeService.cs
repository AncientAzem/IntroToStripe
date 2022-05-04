using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using StripeDemo.Models;

namespace StripeDemo.Services
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StripeService : ControllerBase
    {
        [HttpGet]
        public StripeList<Product> GetAllProducts()
        {
            return new ProductService().List();
        }

        [HttpGet]
        public StripeList<Price> GetPriceForProduct(Product item)
        {
            var options = new PriceListOptions()
            {
                Product = item.Id
            };
            return new PriceService().List(options);
        }

        [HttpGet]
        public List<SessionLineItemOptions> GenerateSessionLineItems(bool onlySubscriptions = false)
        {
            var sessionCart = new List<SessionLineItemOptions>();

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

        [HttpPost]
        public void AddItemsToUpcomingInvoice(string customerId)
        {
            var products = GetAllProducts();
            var service = new InvoiceItemService();
            foreach (var product in products)
            {
                var price = GetPriceForProduct(product).FirstOrDefault();
                if (price is not null && price.Type == "one_time")
                {
                    service.Create(new InvoiceItemCreateOptions()
                    {
                        Customer = customerId,
                        Price = price.Id,
                    });
                    service.Create(new InvoiceItemCreateOptions()
                    {
                        Customer = customerId,
                        Price = price.Id,
                    });
                }
            }
        }

        [HttpGet]
        public string CreateNewConnectAccount(string emailAddress)
        {
            var accountService = new AccountService();
            var stripeAccount = accountService.Create(new AccountCreateOptions()
            {
                Type = "express",
                Country = "US",
                Email = emailAddress,
                Capabilities = new AccountCapabilitiesOptions
                {
                    CardPayments = new AccountCapabilitiesCardPaymentsOptions
                    {
                        Requested = true,
                    },
                    Transfers = new AccountCapabilitiesTransfersOptions
                    {
                        Requested = true,
                    },
                },
            });

            return new AccountLinkService().Create(new AccountLinkCreateOptions()
            {
                Account = stripeAccount.Id,
                RefreshUrl = HttpContext.Request.GetDisplayUrl(),
                ReturnUrl = HttpContext.Request.GetDisplayUrl(),
                Type = "account_onboarding"
            }).Url;
        }

        [HttpGet]
        public string GetAccountManagementLink(string connectUserId)
        {
            return new LoginLinkService().Create(connectUserId).Url;
        }
    }
}