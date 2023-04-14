using Microsoft.AspNetCore.Mvc;

namespace Kite.Gateway.Admin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Main()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
    }
}
