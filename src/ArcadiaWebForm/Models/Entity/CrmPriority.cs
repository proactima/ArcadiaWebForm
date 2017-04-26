using Newtonsoft.Json;

namespace ArcadiaWebForm.Models.Entity
{
    public class CrmPriority : Entity
    {
        [JsonProperty(PropertyName = "isdefault")]
        public bool IsDefault { get; set; }
        public override string Objectname => "crmpriority";
    }
}
