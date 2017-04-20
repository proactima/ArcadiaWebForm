using Newtonsoft.Json;

namespace ArcadiaWebForm.Models
{
    public class Opportunity : BaseModel
    {
        [JsonRequired]
        public string title { get; set; }
        public string description { get; set; }
        [JsonProperty(PropertyName = "e_status_ids")] // crmstatus
        public ArcadiaLink status { get; set; }
        [JsonProperty(PropertyName = "e_client_ids")] // organisation
        public ArcadiaLink client { get; set; }
        [JsonIgnore]
        public string selectedclient { get; set; }
    }
}
