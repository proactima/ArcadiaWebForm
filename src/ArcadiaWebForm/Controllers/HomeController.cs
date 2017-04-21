using ArcadiaWebForm.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace ArcadiaWebForm.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IAccessTokenHandler _accessHandler;

        public HomeController(IConfiguration configuration, IAccessTokenHandler accessHandler)
        {
            _configuration = configuration;
            _accessHandler = accessHandler;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }
    }
}
