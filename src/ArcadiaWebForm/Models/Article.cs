using Newtonsoft.Json;

namespace ArcadiaWebForm.Models
{
    public abstract class Article : BaseModel
    {
        [JsonProperty(PropertyName = "parenttype")]
        public string ParentType { get; set; } = "";

        [JsonProperty(PropertyName ="sys_template")]
        public abstract string TemplateId { get; }
    }
}
