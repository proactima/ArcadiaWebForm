using Newtonsoft.Json;

namespace ArcadiaWebForm.Models
{
    public class ArcadiaLink
    {
        [JsonRequired]
        public string type { get; set; }
        [JsonRequired]
        public string[] values { get; set; }
    }
}
