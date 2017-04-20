using Newtonsoft.Json;

namespace ArcadiaWebForm.Models
{
    public abstract class BaseModel
    {
        [JsonIgnore]
        public string Objectname { get; set; }
        [JsonRequired]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "parentid")]
        public string ParentId => "0";
        [JsonProperty(PropertyName = "parenttype")]
        public string ParentType => "";
    }
}
