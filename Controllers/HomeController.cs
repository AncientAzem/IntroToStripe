using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StripeDemo.Models;
using StripeDemo.Services;
using Stripe;
using Stripe.Checkout;

namespace StripeDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private StripeService _stripe = new StripeService();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(bool? connectRedirect)
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
            ViewData["connectRedirect"] = connectRedirect ?? false;
            return View(productDataList);
        }
        
        public IActionResult StartCheckout(string returnUrl, bool isSubscription = false, bool forConnectAccount = false)
        {
            var sessionOptions = new SessionCreateOptions()
            {
                LineItems = _stripe.GenerateSessionLineItems(isSubscription),
                PaymentMethodTypes = new List<string>() { "card" },
                Mode = isSubscription ? "subscription" : "payment",
                SuccessUrl = returnUrl,
                CancelUrl = returnUrl,
            };
            if (forConnectAccount && !isSubscription)
            {
                sessionOptions.PaymentIntentData = new SessionPaymentIntentDataOptions()
                {
                    ApplicationFeeAmount = 200,
                    TransferData = new SessionPaymentIntentDataTransferDataOptions()
                    {
                        Destination = "acct_1KvosXQWS9VMSEWG"
                    }
                };
            }

            Session session = new SessionService().Create(sessionOptions);
            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public IActionResult GenerateInvoice(bool forConnectAccount = false)
        {
            _stripe.AddItemsToUpcomingInvoice("cus_Ld7MhXFvXpHObz");
            var invoiceOptions = new InvoiceCreateOptions()
            {
                Customer = "cus_Ld7MhXFvXpHObz",
                CollectionMethod = "send_invoice",
                DaysUntilDue = 7
            };
            if (forConnectAccount)
            {
                invoiceOptions.ApplicationFeeAmount = 150;
                invoiceOptions.OnBehalfOf = "acct_1KvosXQWS9VMSEWG";
                invoiceOptions.TransferData = new InvoiceTransferDataOptions()
                {
                    Destination = "acct_1KvosXQWS9VMSEWG"
                };
            }
            
            var service = new InvoiceService();
            var invoice = service.Create(invoiceOptions);
            invoice = service.FinalizeInvoice(invoice.Id);
            Response.Headers.Add("Location", invoice.HostedInvoiceUrl);
            return new StatusCodeResult(303);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}