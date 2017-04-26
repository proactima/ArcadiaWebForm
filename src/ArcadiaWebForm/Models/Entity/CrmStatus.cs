using Newtonsoft.Json;

namespace ArcadiaWebForm.Models.Entity
{
    public class CrmStatus : Entity
    {
        [JsonProperty(PropertyName = "indraft")]
        public bool InDraft { get; set; }

        public override string Objectname => "crmstatus";
    }
}
