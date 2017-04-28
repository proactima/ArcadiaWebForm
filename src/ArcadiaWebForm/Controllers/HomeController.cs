using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArcadiaWebForm.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Success()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return RedirectToAction("Signout", "Account");
        }
    }
}
