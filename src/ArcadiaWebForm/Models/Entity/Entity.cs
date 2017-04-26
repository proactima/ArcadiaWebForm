using Newtonsoft.Json;

namespace ArcadiaWebForm.Models.Entity
{
    public abstract class Entity : BaseModel
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
