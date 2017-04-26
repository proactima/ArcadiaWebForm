using Newtonsoft.Json;

namespace ArcadiaWebForm.Models
{
    public abstract class BaseModel
    {
        [JsonIgnore]
        public abstract string Objectname { get; }

        [JsonRequired]
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "parentid")]
        public string ParentId { get; set; } = "0";
    }
}
