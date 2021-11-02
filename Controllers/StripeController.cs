using Microsoft.AspNetCore.Mvc;

namespace StripeDemo.Controllers
{
    public class StripeController : Controller
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}