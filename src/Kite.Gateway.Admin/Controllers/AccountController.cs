using Microsoft.AspNetCore.Mvc;

namespace Kite.Gateway.Admin.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
