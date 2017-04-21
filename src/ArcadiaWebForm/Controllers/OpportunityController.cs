using ArcadiaWebForm.Models;
using ArcadiaWebForm.Models.Opportunity;
using ArcadiaWebForm.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArcadiaWebForm.Controllers
{
    public class OpportunityController : Controller
    {
        private readonly ICallApi _apiCaller;
        private readonly IMapper _map;

        public OpportunityController(ICallApi apiCaller, IMapper map)
        {
            _apiCaller = apiCaller;
            _map = map;
        }

        [HttpGet]
        public async Task<IActionResult> New()
        {
            var id = await _apiCaller.GetId();

            var opportunity = CreateViewMode(id);
            return View("New", opportunity);
        }

        private View CreateViewMode(string id)
        {
            var opportunity = new View
            {
                Id = id,
                ExpectedInput = new Input(),
                ClientList = new List<SelectListItem>
                {   // todo get from API!
                    new SelectListItem { Text = "Select a client", Value = "", Selected = true},
                    new SelectListItem { Text = "ABB", Value = "abb_id" }
                }
            };
            return opportunity;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id, Input obj)
        {
            if (!ModelState.IsValid)
            {
                var opportunity = CreateViewMode(id);
                return View("New", opportunity);
            }

            var outputObj = _map.Map<Input, Output>(obj);

            var crmStatuses = await _apiCaller.LoadEntities<CrmStatus>("crmstatus");
            var statusId = crmStatuses.First(c => c.IsDraft).Id;
            outputObj.Status = new ArcadiaLink { Type = "crmstatus", Values = new[] { statusId } };

            var response = await _apiCaller.StoreArticleAsync(outputObj);

            return RedirectToAction("index", "Forms");
        }
    }
}