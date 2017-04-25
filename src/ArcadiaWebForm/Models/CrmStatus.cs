using System;
using Newtonsoft.Json;

namespace ArcadiaWebForm.Models
{
    public class CrmStatus : Entity
    {
        [JsonProperty(PropertyName = "indraft")]
        public bool InDraft { get; set; }

        public override string Objectname => "crmstatus";
    }
}
