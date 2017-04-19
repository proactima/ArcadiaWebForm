using Newtonsoft.Json;

namespace ArcadiaWebForm.Models
{
    public class Opportunity
    {
        public Opportunity(string objectId, string objectTitle)
        {
            id = objectId;
            title = objectTitle;
        }
        public string id { get; }
        public string parentid => "0";
        public string parenttype => "";
        public string title { get; }
        public string description { get; set; }

        [JsonProperty(PropertyName ="e_status_ids")] // crmstatus
        public ArcadiaLink status { get; set; }

        [JsonProperty(PropertyName = "e_client_ids")] // organisation
        public ArcadiaLink client { get; set; }
    }

    public class ArcadiaLink
    {
        public string type { get; set; }
        public string[] values { get; set; }
    }
}
