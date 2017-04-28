using ArcadiaWebForm.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ArcadiaWebForm.Controllers
{
    public class EntityController : Controller
    {
        private readonly ICallApi _apiCaller;
        public EntityController(ICallApi apiCaller)
        {
            _apiCaller = apiCaller;
        }

        public async Task<JsonResult> Get(string objectname)
        {
            var clients = await _apiCaller.LoadAllEntities(objectname);
            return Json(clients);
        }
    }
}
