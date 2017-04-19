using Newtonsoft.Json;

namespace ArcadiaWebForm.Models
{
    public class Opportunity
    {
        [JsonRequired]
        public string id { get; set; }
        public string parentid => "0";
        public string parenttype => "";
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

    public class ArcadiaLink
    {
        [JsonRequired]
        public string type { get; set; }
        [JsonRequired]
        public string[] values { get; set; }
    }
}
