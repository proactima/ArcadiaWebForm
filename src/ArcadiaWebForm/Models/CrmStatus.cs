using Newtonsoft.Json;

namespace ArcadiaWebForm.Models
{
    public class CrmStatus : Entity
    {
        [JsonProperty(PropertyName = "isdraft")]
        public bool IsDraft { get; set; }
    }
}
