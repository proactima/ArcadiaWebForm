using ArcadiaWebForm.Models;
using ArcadiaWebForm.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ArcadiaWebForm.Controllers
{
    public class FormsController : Controller
    {
        ICallApi _api;
        public FormsController(ICallApi api)
        {
            _api = api;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Opportunity()
        {
            var id = await _api.GetId();
            var opportunity = new Opportunity { Id = id };
            return View(opportunity);
        }
    }
}