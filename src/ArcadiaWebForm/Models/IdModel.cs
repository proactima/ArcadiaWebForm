using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArcadiaWebForm.Models
{
    public class IdModel
    {
        [JsonProperty(PropertyName = "results")]
        public IList<InnerId> Results { get; set; }
    }

    public class InnerId
    {
        [JsonProperty(PropertyName ="ids")]
        public IList<string> Ids { get; set; }
    }
}
