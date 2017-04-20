using ArcadiaWebForm.Models;
using ArcadiaWebForm.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace ArcadiaWebForm.Controllers
{
    public class OpportunityController : Controller
    {
        private readonly ICallApi _apiCaller;
        public OpportunityController(ICallApi apiCaller)
        {
            _apiCaller = apiCaller;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id, [Bind("id, title, description, selectedclient")] Opportunity obj)
        {
            if (!ModelState.IsValid) return View("Opportunity", obj);
            var response = await _apiCaller.StoreArticleAsync(obj);
            if (response.StatusCode != HttpStatusCode.OK) return View("Opportunity", obj);
            return RedirectToAction("index", "Forms");
        }
    }
}