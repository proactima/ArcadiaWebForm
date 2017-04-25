using ArcadiaWebForm.Models;
using ArcadiaWebForm.Models.Opportunity;
using ArcadiaWebForm.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
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
            var clients = await _apiCaller.LoadEntities<Organisation>();

            var selectableClients = clients
                .Select(c => new SelectListItem { Text = c.Name, Value = c.Id })
                .OrderBy(c => c.Text)
                .Concat(new[] { new SelectListItem { Text = "Select a client", Value = "", Selected = true } })
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

            await HandleDefaultValuesAsync(outputObj);

            var response = await _apiCaller.StoreArticleAsync(outputObj);

            return RedirectToAction("index", "Forms");
        }

        private async Task HandleDefaultValuesAsync(Output outputObj)
        {
            outputObj.Status = await CreateLinkObject<CrmStatus>(c => c.InDraft);
            outputObj.Probability = await CreateLinkObject<CrmProbability>(c => c.IsDefault);
            outputObj.Phase = await CreateLinkObject<CrmPhase>(c => c.IsDefault);
            outputObj.Priority = await CreateLinkObject<CrmPriority>(c => c.IsDefault);
        }

        private async Task<ArcadiaLink> CreateLinkObject<T>(Func<T, bool> predicateForDefaultValue) where T : Entity, new()
        {
            var objects = await _apiCaller.LoadEntities<T>();
            var defaultValue = objects.First(predicateForDefaultValue);
            return new ArcadiaLink { Type = defaultValue.Objectname, Values = new[] { defaultValue.Id } };
        }
    }
}