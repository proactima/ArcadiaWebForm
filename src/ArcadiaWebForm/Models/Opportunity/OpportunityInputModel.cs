using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ArcadiaWebForm.Models.Opportunity
{
    public class OpportunityInputModel
    {
        [HiddenInput]
        public string Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        [Display(Name ="Client")]
        public string SelectedClient { get; set; }
    }
}
