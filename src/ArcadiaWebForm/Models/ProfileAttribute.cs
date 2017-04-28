using ArcadiaWebForm.Controllers;
using ArcadiaWebForm.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Threading.Tasks;

namespace ArcadiaWebForm.Models
{
    public class ProfileAttribute : ActionFilterAttribute
    {
        private readonly ICallApi _api;
        public ProfileAttribute(ICallApi api)
        {
            _api = api;
        }
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var controller = context.Controller as Controller;

            if (controller == null || controller.GetType() == typeof(AccountController) || !controller.HttpContext.User.Identity.IsAuthenticated)
            {
                await base.OnActionExecutionAsync(context, next);
                return;
            }

            try
            {
                var profile = await _api.GetUserProfile();

                controller.ViewBag.OrgPrefix = profile.CurrentOrganization.Prefix;
                controller.ViewBag.OrgName = profile.CurrentOrganization.Name;
                controller.ViewBag.SharedUserId = profile.ShareduserId;

                await base.OnActionExecutionAsync(context, next);
            }
            catch (AdalException)
            {
                context.Result = new RedirectToActionResult("Signout", "Account", null);
            }
        }
    }
}
