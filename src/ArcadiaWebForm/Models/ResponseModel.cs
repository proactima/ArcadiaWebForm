using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArcadiaWebForm.Models
{
    public class ResponseModel<T>
    {
        [JsonProperty(PropertyName = "results")]
        public IList<T> Results { get; set; }
    }
}
