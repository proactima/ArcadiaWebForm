using Newtonsoft.Json;

namespace ArcadiaWebForm.Models
{
    public abstract class Article : BaseModel
    {
        [JsonProperty(PropertyName = "parenttype")]
        public string ParentType { get; set; } = "";

        [JsonRequired]
        [JsonProperty(PropertyName ="sys_template")]
        public abstract string TemplateId { get; }

        [JsonProperty(PropertyName = "etag")]
        public string ETag { get; set; } = "";
    }
}
