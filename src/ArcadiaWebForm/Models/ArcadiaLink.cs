using Newtonsoft.Json;

namespace ArcadiaWebForm.Models
{
    public class ArcadiaLink
    {
        [JsonRequired]
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonRequired]
        [JsonProperty(PropertyName = "values")]
        public string[] Values { get; set; }
    }
}
