using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArcadiaWebForm.Models
{
    public class IdModel
    {
        [JsonProperty(PropertyName ="ids")]
        public IList<string> Ids { get; set; }
    }
}
