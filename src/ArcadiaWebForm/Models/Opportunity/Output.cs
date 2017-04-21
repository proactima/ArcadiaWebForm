using Newtonsoft.Json;

namespace ArcadiaWebForm.Models.Opportunity
{
    public class Output : Article
    {
        [JsonRequired]
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "e_status_ids")] // crmstatus
        public ArcadiaLink Status { get; set; }

        [JsonProperty(PropertyName = "e_client_ids")] // organisation
        public ArcadiaLink Client { get; set; }

    }
}
