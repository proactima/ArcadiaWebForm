using ArcadiaWebForm.Models;
using ArcadiaWebForm.Models.Opportunity;
using ArcadiaWebForm.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

            var opportunity = await CreateViewModeAsync(id);
            return View("New", opportunity);
        }

        private async Task<View> CreateViewModeAsync(string id)
        {
            var clients = await _apiCaller.LoadEntities<Organisation>(new Organisation().Objectname);

            var selectableClients = clients
                .Select(c => new SelectListItem { Text = c.Name, Value = c.Id })
                .OrderBy(c => c.Text)
                .Concat(new[] { new SelectListItem { Text = "Select an organisation", Value = "", Selected = true } })
                .OrderByDescending(s => s.Selected)
                .ToList();

            var opportunity = new View
            {
                Id = id,
                ExpectedInput = new Input(),
                ClientList = selectableClients
            };
            return opportunity;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id, Input obj)
        {
            if (!ModelState.IsValid)
            {
                var opportunity = await CreateViewModeAsync(id);
                return View("New", opportunity);
            }

            var outputObj = _map.Map<Input, Output>(obj);

            var crmStatuses = await _apiCaller.LoadEntities<CrmStatus>("crmstatus");
            var statusId = crmStatuses.First(c => c.InDraft).Id;
            outputObj.Status = new ArcadiaLink { Type = "crmstatus", Values = new[] { statusId } };

            var response = await _apiCaller.StoreArticleAsync(outputObj);

            return RedirectToAction("index", "Forms");
        }
    }
}