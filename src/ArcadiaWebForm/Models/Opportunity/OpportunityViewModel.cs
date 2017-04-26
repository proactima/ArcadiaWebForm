using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace ArcadiaWebForm.Models.Opportunity
{
    public class OpportunityViewModel
    {
        public string Id { get; set; }
        public IList<SelectListItem> ClientList { get; set; }
        public OpportunityInputModel ExpectedInput { get; set; }
    }
}
