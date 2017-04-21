using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ArcadiaWebForm.Models.Opportunity
{
    public class View
    {
        public string Id { get; set; }
        public IList<SelectListItem> ClientList { get; set; }
        public Input ExpectedInput { get; set; }
    }
}
