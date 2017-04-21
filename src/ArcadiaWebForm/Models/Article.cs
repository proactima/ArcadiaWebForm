using Newtonsoft.Json;

namespace ArcadiaWebForm.Models
{
    public class Article : BaseModel
    {
        [JsonProperty(PropertyName = "parenttype")]
        public string ParentType => "";
    }
}
