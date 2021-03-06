using ArcadiaWebForm.Models;
using ArcadiaWebForm.Models.Entity;
using ArcadiaWebForm.Models.Opportunity;
using ArcadiaWebForm.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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

            var opportunity = CreateViewModel(id);
            return View("New", opportunity);
        }

        private OpportunityViewModel CreateViewModel(string id)
        {
            var opportunity = new OpportunityViewModel
            {
                Id = id,
                ExpectedInput = new OpportunityInputModel(),
            };
            return opportunity;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string id, OpportunityInputModel obj)
        {
            if (!ModelState.IsValid)
            {
                var opportunity = CreateViewModel(id);
                return View("New", opportunity);
            }

            var outputObj = _map.Map<OpportunityInputModel, OpportunityOutputModel>(obj);

            await HandleDefaultValuesAsync(outputObj);

            var response = await _apiCaller.StoreArticleAsync(outputObj);

            return RedirectToAction("success", "Home");
        }

        private async Task HandleDefaultValuesAsync(OpportunityOutputModel outputObj)
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