using Newtonsoft.Json;

namespace ArcadiaWebForm.Models.Opportunity
{
    public class OpportunityOutputModel : Article
    {
        [JsonRequired]
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "e_status_ids")]
        public ArcadiaLink Status { get; set; }

        [JsonProperty(PropertyName = "e_priority_ids")]
        public ArcadiaLink Priority { get; set; }

        [JsonProperty(PropertyName = "e_phase_ids")]
        public ArcadiaLink Phase { get; set; }

        [JsonProperty(PropertyName = "e_probability_ids")]
        public ArcadiaLink Probability { get; set; }

        [JsonProperty(PropertyName = "e_client_ids")]
        public ArcadiaLink Client { get; set; }

        public override string Objectname => "crm";
        public override string TemplateId => "7a283a1d-e11e-40a1-b4f7-db0eb876ae76";
    }
}
