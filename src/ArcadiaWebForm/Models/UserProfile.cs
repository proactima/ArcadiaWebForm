using Newtonsoft.Json;
using System.Collections.Generic;

namespace ArcadiaWebForm.Models
{
    public class UserProfile : BaseModel
    {
        [JsonProperty(PropertyName = "shareduserid")]
        public string ShareduserId { get; set; }

        [JsonProperty(PropertyName = "tmp_mailaddress")]
        public string MailAddress { get; set; }

        [JsonProperty(PropertyName = "tmp_currenttenant")]
        public OrganizationInfo CurrentOrganization { get; set; }

        [JsonProperty(PropertyName = "tmp_tenants")]
        public IList<OrganizationInfo> AccessOrganizations { get; set; }

        public override string Objectname => "user";
    }

    public class OrganizationInfo
    {
        [JsonProperty(PropertyName = "prefix")]
        public string Prefix { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
