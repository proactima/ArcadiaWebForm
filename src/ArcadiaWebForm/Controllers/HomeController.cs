using ArcadiaWebForm.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

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

        public async Task<IActionResult> About()
        {
            string accessToken = await _accessHandler.AquireAccessTokenAsync();

            var baseUrl = _configuration["Settings:BaseUrl"];
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, baseUrl + "/entity");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await client.SendAsync(request);

            ViewData["Message"] = $"Response was: {response.StatusCode}";

            return View();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }
    }
}
